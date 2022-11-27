//Simplified BSD License (BSD-2-Clause)
//
//Copyright (c) 2020, Christopher Lees, The OpenBVE Project
//
//Redistribution and use in source and binary forms, with or without
//modification, are permitted provided that the following conditions are met:
//
//1. Redistributions of source code must retain the above copyright notice, this
//   list of conditions and the following disclaimer.
//2. Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
//
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
//ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.IO;
using System.Linq;
using System.Text;
using OpenBve.Formats.DirectX;
using OpenBveApi.Colors;
using OpenBveApi.Interface;
using OpenBveApi.Math;
using OpenBveApi.Objects;

namespace Plugin
{
	class NewXParser
	{
		internal static StaticObject ReadObject(string FileName, Encoding Encoding)
		{
			rootMatrix = Matrix4D.NoTransformation;
			currentFolder = System.IO.Path.GetDirectoryName(FileName);
			currentFile = FileName;
			byte[] Data = System.IO.File.ReadAllBytes(FileName);
			
			// floating-point format
			int FloatingPointSize;
			if (Data[12] == 48 & Data[13] == 48 & Data[14] == 51 & Data[15] == 50)
			{
				FloatingPointSize = 32;
			}
			else if (Data[12] == 48 & Data[13] == 48 & Data[14] == 54 & Data[15] == 52)
			{
				FloatingPointSize = 64;
			}
			else
			{
				throw new NotSupportedException();
			}

			// supported floating point format
			if (Data[8] == 116 & Data[9] == 120 & Data[10] == 116 & Data[11] == 32)
			{
				// textual flavor
				string[] Lines = File.ReadAllLines(FileName, Encoding);
				// strip away comments
				bool Quote = false;
				for (int i = 0; i < Lines.Length; i++) {
					for (int j = 0; j < Lines[i].Length; j++) {
						if (Lines[i][j] == '"') Quote = !Quote;
						if (!Quote) {
							if (Lines[i][j] == '#' || j < Lines[i].Length - 1 && Lines[i].Substring(j, 2) == "//") {
								Lines[i] = Lines[i].Substring(0, j);
								break;
							}
						}
					}
					//Convert runs of whitespace to single
					var list = Lines[i].Split(new char[] { }).Where(s => !string.IsNullOrWhiteSpace(s));
					Lines[i] = string.Join(" ", list);
				}
				StringBuilder Builder = new StringBuilder();
				for (int i = 0; i < Lines.Length; i++) {
					Builder.Append(Lines[i]);
					Builder.Append(" ");
				}
				string Content = Builder.ToString();
				Content = Content.Substring(17).Trim(new char[] { });
				return LoadTextualX(Content);
			}

			byte[] newData;
			if (Data[8] == 98 & Data[9] == 105 & Data[10] == 110 & Data[11] == 32)
			{
				//Uncompressed binary, so skip the header
				newData = new byte[Data.Length - 16];
				Array.Copy(Data, 16, newData, 0, Data.Length - 16);
				return LoadBinaryX(newData, FloatingPointSize);
			}

			if (Data[8] == 116 & Data[9] == 122 & Data[10] == 105 & Data[11] == 112)
			{
				// compressed textual flavor
				newData = MSZip.Decompress(Data);
				string Text = Encoding.GetString(newData);
				return LoadTextualX(Text);
			}

			if (Data[8] == 98 & Data[9] == 122 & Data[10] == 105 & Data[11] == 112)
			{
				//Compressed binary
				//16 bytes of header, then 8 bytes of padding, followed by the actual compressed data
				byte[] Uncompressed = MSZip.Decompress(Data);
				return LoadBinaryX(Uncompressed, FloatingPointSize);
			}

			// unsupported flavor
			Plugin.currentHost.AddMessage(MessageType.Error, false, "Unsupported X object file encountered in " + FileName);
			return null;
		}
		
		private static StaticObject LoadTextualX(string Text)
		{
			Text = Text.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("\t", " ").Trim(new char[] { });
			StaticObject obj = new StaticObject(Plugin.currentHost);
			MeshBuilder builder = new MeshBuilder(Plugin.currentHost);
			Material material = new Material();
			Block block = new TextualBlock(Text);
			while (block.Position() < block.Length() - 5)
			{
				Block subBlock = block.ReadSubBlock();
				ParseSubBlock(subBlock, ref obj, ref builder, ref material);
			}
			builder.Apply(ref obj);
			obj.Mesh.CreateNormals();
			if (rootMatrix != Matrix4D.NoTransformation)
			{
				for (int i = 0; i < obj.Mesh.Vertices.Length; i++)
				{
					obj.Mesh.Vertices[i].Coordinates.Transform(rootMatrix);
				}
			}
			return obj;
		}

		private static string currentFolder;
		private static string currentFile;

		private static Matrix4D rootMatrix;
		private static int currentLevel = 0;

		private static void ParseSubBlock(Block block, ref StaticObject obj, ref MeshBuilder builder, ref Material material)
		{
			Block subBlock;
			switch (block.Token)
			{
				default:
					return;
				case TemplateID.Template:
					string GUID = block.ReadString();
					/*
					 * Valid Microsoft templates are listed here:
					 * https://docs.microsoft.com/en-us/windows/desktop/direct3d9/dx9-graphics-reference-x-file-format-templates
					 * However, an application may define it's own template (or by the looks of things override another)
					 * by declaring this at the head of the file, and using a unique GUID
					 *
					 * Mesquoia does this by defining a copy of the Boolean template using a WORD as opposed to a DWORD
					 * No practical effect in this case, however be wary of this....
					 */
					return;
				case TemplateID.Header:
					int majorVersion = block.ReadUInt16();
					int minorVersion = block.ReadUInt16();
					int flags = block.ReadUInt16();
					switch (flags)
					{
						/* According to http://paulbourke.net/dataformats/directx/#xfilefrm_Template_Header
						 * it is possible for a file to contain a mix of both binary and textual blocks.
						 *
						 * The Header block controls the format of the file from this point onwards.
						 * majorVersion and minorVersion relate to the legacy Direct3D retained mode API
						 * and can probably be ignored. (Assume that features are cumulative and backwards compatible)
						 * flags sets whether the blocks from this point onwards are binary or textual.
						 *
						 * TODO: Need a mixed mode file sample if we want this to work.
						 * Probably exceedingly uncommon, so low priority
						 */

						case 0:
							if (block is TextualBlock)
							{
								throw new Exception("Mixed-mode text and binary objects are not supported by this parser.");
							}
							break;
						default:
							if (block is BinaryBlock)
							{
								throw new Exception("Mixed-mode text and binary objects are not supported by this parser.");
							}
							break;
					}
					return;
				case TemplateID.Frame:
					currentLevel++;
					if (builder.Vertices.Count != 0)
					{
						builder.Apply(ref obj);
						builder = new MeshBuilder(Plugin.currentHost);
					}
					while (block.Position() < block.Length() - 5)
					{
						/*
						 * TODO: Whilst https://docs.microsoft.com/en-us/windows/desktop/direct3d9/frame suggests the Frame template should only contain
						 * Mesh, FrameTransformMatrix or Frame templates by default, 3DS Max stuffs all manner of things into here
						 *
						 * It would be nice to get 3DS max stuff detected specifically, especially as we don't support most of this
						 */
						//TemplateID[] validTokens = { TemplateID.Mesh , TemplateID.FrameTransformMatrix, TemplateID.Frame };
						subBlock = block.ReadSubBlock();
						ParseSubBlock(subBlock, ref obj, ref builder, ref material);
					}
					currentLevel--;
					break;
				case TemplateID.FrameTransformMatrix:
					double[] matrixValues = new double[16];
					for (int i = 0; i < 16; i++)
					{
						matrixValues[i] = block.ReadSingle();
					}

					if (currentLevel > 1)
					{
						builder.TransformMatrix = new Matrix4D(matrixValues);
					}
					else
					{
						rootMatrix = new Matrix4D(matrixValues);
					}
					break;
				case TemplateID.Mesh:
					if (builder.Vertices.Count != 0)
					{
						builder.Apply(ref obj);
						builder = new MeshBuilder(Plugin.currentHost);
					}
					int nVerts = block.ReadUInt16();
					if (nVerts == 0)
					{
						//Some null objects contain an empty mesh
						Plugin.currentHost.AddMessage(MessageType.Warning, false, "nVertices should be greater than zero in Mesh " + block.Label);
					}
					for (int i = 0; i < nVerts; i++)
					{
						builder.Vertices.Add(new Vertex(new Vector3(block.ReadSingle(), block.ReadSingle(), block.ReadSingle())));
					}
					int nFaces = block.ReadUInt16();
					if (nFaces == 0)
					{
						try
						{
							/*
							 * A mesh has been defined with no faces.
							 * If we are not at the end of the block,
							 * attempt to read the next sub-block
							 *
							 * If this fails, the face count is probably incorrect
							 *
							 * NOTE: In this case, the face statement will be an empty string / whitespace
							 * hence the block.ReadString() call
							 */
							block.ReadString();
							if (block.Position() < block.Length() - 5)
							{
								subBlock = block.ReadSubBlock();
								ParseSubBlock(subBlock, ref obj, ref builder, ref material);
							}
							goto NoFaces;
						}
						catch
						{
							throw new Exception("nFaces was declared as zero, but unrecognised data remains in the block");
						}
						
					}
					for (int i = 0; i < nFaces; i++)
					{
						int fVerts = block.ReadUInt16();
						if (nFaces == 0)
						{
							throw new Exception("fVerts must be greater than zero");
						}
						MeshFace f = new MeshFace();
						f.Vertices = new MeshFaceVertex[fVerts];
						for (int j = 0; j < fVerts; j++)
						{
							f.Vertices[j].Index = block.ReadUInt16();
						}
						builder.Faces.Add(f);
					}
					NoFaces:
					while (block.Position() < block.Length() - 5)
					{
						subBlock = block.ReadSubBlock();
						ParseSubBlock(subBlock, ref obj, ref builder, ref material);
					}
					break;
				case TemplateID.MeshMaterialList:
					int nMaterials = block.ReadUInt16();
					int nFaceIndices = block.ReadUInt16();
					if (nFaceIndices == 1 && builder.Faces.Count > 1)
					{
						//Single material for all faces
						int globalMaterial = block.ReadUInt16();
						for (int i = 0; i < builder.Faces.Count; i++)
						{
							MeshFace f = builder.Faces[i];
							f.Material = (ushort)(globalMaterial + 1);
							builder.Faces[i] = f;
						}
					}
					else if(nFaceIndices == builder.Faces.Count)
					{
						for (int i = 0; i < nFaceIndices; i++)
						{
							int fMaterial = block.ReadUInt16();
							MeshFace f = builder.Faces[i];
							f.Material = (ushort) (fMaterial + 1);
							builder.Faces[i] = f;
						}
					}
					else
					{
						throw new Exception("nFaceIndices must match the number of faces in the mesh");
					}
					for (int i = 0; i < nMaterials; i++)
					{
						subBlock = block.ReadSubBlock(TemplateID.Material);
						ParseSubBlock(subBlock, ref obj, ref builder, ref material);
					}
					break;
				case TemplateID.Material:
					int m = builder.Materials.Length;
					Array.Resize(ref builder.Materials, m + 1);
					builder.Materials[m] = new Material();
					builder.Materials[m].Color = new Color32((byte)(255 * block.ReadSingle()), (byte)(255 * block.ReadSingle()), (byte)(255 * block.ReadSingle()),(byte)(255 * block.ReadSingle()));
					double mPower = block.ReadSingle(); //TODO: Unsure what this does...
					Color24 mSpecular = new Color24((byte)block.ReadSingle(), (byte)block.ReadSingle(), (byte)block.ReadSingle());
					builder.Materials[m].EmissiveColor = new Color24((byte)(255 *block.ReadSingle()), (byte)(255 * block.ReadSingle()), (byte)(255 * block.ReadSingle()));
					builder.Materials[m].Flags |= MaterialFlags.Emissive; //TODO: Check exact behaviour
					if (Plugin.EnabledHacks.BlackTransparency)
					{
						builder.Materials[m].TransparentColor = Color24.Black; //TODO: Check, also can we optimise which faces have the transparent color set?
						builder.Materials[m].Flags |= MaterialFlags.TransparentColor;
					}
					
					if (block.Position() < block.Length() - 5)
					{
						subBlock = block.ReadSubBlock(TemplateID.TextureFilename);
						ParseSubBlock(subBlock, ref obj, ref builder, ref builder.Materials[m]);
					}
					break;
				case TemplateID.TextureFilename:
					string texturePath = block.ReadString();

					// If the specified file name is an absolute path, make it the file name only.
					// Some object files specify absolute paths.
					// And BVE4/5 doesn't allow textures to be placed in a different directory than the object file.
					if (Plugin.EnabledHacks.BveTsHacks && OpenBveApi.Path.IsAbsolutePath(texturePath))
					{
						texturePath = texturePath.Split('/', '\\').Last();
					}

					try
					{
						material.DaytimeTexture = OpenBveApi.Path.CombineFile(currentFolder, texturePath);
					}
					catch (Exception e)
					{
						Plugin.currentHost.AddMessage(MessageType.Error, false, $"Texture file path {texturePath} in file {currentFile} has the problem: {e.Message}");
						material.DaytimeTexture = null;
					}

					if (!System.IO.File.Exists(material.DaytimeTexture) && material.DaytimeTexture != null)
					{
						Plugin.currentHost.AddMessage(MessageType.Error, true, "Texure " + material.DaytimeTexture + " was not found in file " + currentFile);
						material.DaytimeTexture = null;
					}
					break;
				case TemplateID.MeshTextureCoords:
					int nCoords = block.ReadUInt16();
					for (int i = 0; i < nCoords; i++)
					{
						builder.Vertices[i].TextureCoordinates = new Vector2(block.ReadSingle(), block.ReadSingle());
					}
					break;
				case TemplateID.MeshNormals:
					int nNormals = block.ReadUInt16();
					Vector3[] normals = new Vector3[nNormals];
					for (int i = 0; i < nNormals; i++)
					{
						normals[i] = new Vector3(block.ReadSingle(), block.ReadSingle(), block.ReadSingle());
						normals[i].Normalize();
					}
					int nFaceNormals = block.ReadUInt16();
					if (nFaceNormals != builder.Faces.Count)
					{
						throw new Exception("nFaceNormals must match the number of faces in the mesh");
					}
					for (int i = 0; i < nFaceNormals; i++)
					{
						int nVertexNormals = block.ReadUInt16();
						if (nVertexNormals != builder.Faces[i].Vertices.Length)
						{
							throw new Exception("nVertexNormals must match the number of verticies in the face");
						}
						for (int j = 0; j < nVertexNormals; j++)
						{
							builder.Faces[i].Vertices[j].Normal = normals[block.ReadUInt16()];
						}
					}
					break;
				case TemplateID.MeshVertexColors:
					int nVertexColors = block.ReadUInt16();
					for (int i = 0; i < nVertexColors; i++)
					{
						builder.Vertices[i] = new ColoredVertex((Vertex)builder.Vertices[i], new Color128(block.ReadSingle(), block.ReadSingle(), block.ReadSingle()));
					}
					break;
				case TemplateID.MeshFaceWraps:
					int nMeshFaceWraps = block.ReadUInt16();
					if (nMeshFaceWraps != builder.Faces.Count)
					{
						throw new Exception("nMeshFaceWraps must match the number of faces in the mesh");
					}
					/*
					 * MeshFaceWraps is a 2 * boolean array, representing the clamping on X / Y axis for each face
					 * The current engine only supports clamping on a per-texture basis & this was discontinued in
					 * later versions of DirectX so just validate this is structurally valid and ignore for the minute
					 */
					break;
			}
		}

		private static StaticObject LoadBinaryX(byte[] Data, int FloatingPointSize)
		{
			Block block = new BinaryBlock(Data, FloatingPointSize);
			block.FloatingPointSize = FloatingPointSize;
			StaticObject obj = new StaticObject(Plugin.currentHost);
			MeshBuilder builder = new MeshBuilder(Plugin.currentHost);
			Material material = new Material();
			while (block.Position() < block.Length())
			{
				Block subBlock = block.ReadSubBlock();
				ParseSubBlock(subBlock, ref obj, ref builder, ref material);
			}
			builder.Apply(ref obj);
			obj.Mesh.CreateNormals();
			return obj;
		}
	}
}
