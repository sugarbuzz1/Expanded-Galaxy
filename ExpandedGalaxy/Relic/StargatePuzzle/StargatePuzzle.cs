using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExpandedGalaxy
{
    public class StargatePuzzle
    {
        static readonly float PHI = (1f + Mathf.Sqrt(5f)) / 2f;
        static readonly char[] FaceLetters = "αβγδεζηθικλμ§νξοπρστ".ToCharArray();
        internal static readonly char[] SubLetters = "αβγδεζηθικλμ§νξοπρστυφχψω".ToCharArray();

        

        static readonly List<Face> Faces = BuildIcosahedron();

        public static Vector3 GeneratePuzzleVector(out string solution, int seed)
        {
            PLRand rng = new PLRand(seed);

            Face face = Faces[rng.Next(0, Faces.Count)];
            solution = face.Letter.ToString();

            Vector3 A = face.A;
            Vector3 B = face.B;
            Vector3 C = face.C;

            for (int depth = 0; depth < 4; depth++)
            {
                int triIndex = rng.Next(0, 25);
                solution += SubLetters[triIndex];

                GetSubTriangle(A, B, C, triIndex, out A, out B, out C);
            }

            return Vector3.Normalize((A + B + C) / 3f);
        }

        public static string Solve(Vector3 direction)
        {
            direction = direction.normalized;
            Face face = FindFace(direction);

            string code = face.Letter.ToString();

            Vector3 P = RayPlaneHit(direction, face);
            Vector3 A = face.A, B = face.B, C = face.C;

            for (int depth = 0; depth < 4; depth++)
            {
                int triIndex = FindSubTriangleIndex(A, B, C, P);
                code += SubLetters[triIndex];

                GetSubTriangle(A, B, C, triIndex, out A, out B, out C);
            }

            return code;
        }

        public static Vector3 VectorFromCode(string code)
        {
            if (code == null || code.Length != 5)
                return Vector3.zero;

            int faceIndex = Array.IndexOf(FaceLetters, code[0]);
            if (faceIndex < 0)
                return Vector3.zero;

            Face face = Faces[faceIndex];
            Vector3 A = face.A, B = face.B, C = face.C;

            for (int depth = 1; depth < 5; depth++)
            {
                int triIndex = Array.IndexOf(SubLetters, code[depth]);
                if (triIndex < 0)
                    return Vector3.zero;

                GetSubTriangle(A, B, C, triIndex, out A, out B, out C);
            }

            return Vector3.Normalize((A + B + C) / 3f);
        }

        static void GetSubTriangle(
            Vector3 A, Vector3 B, Vector3 C,
            int index,
            out Vector3 tA, out Vector3 tB, out Vector3 tC)
        {
            int count = 0;

            Vector3 AB = (B - A) / 5f;
            Vector3 AC = (C - A) / 5f;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5 - i; j++)
                {
                    Vector3 P = A + i * AB + j * AC;

                    if (count == index)
                    {
                        tA = P;
                        tB = P + AB;
                        tC = P + AC;
                        return;
                    }
                    count++;

                    if (i + j < 4)
                    {
                        if (count == index)
                        {
                            tA = P + AB;
                            tB = P + AB + AC;
                            tC = P + AC;
                            return;
                        }
                        count++;
                    }
                }
            }

            throw new ArgumentOutOfRangeException(nameof(index));
        }

        static int FindSubTriangleIndex(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
        {
            Vector3 AB = B - A;
            Vector3 AC = C - A;
            Vector3 AP = P - A;

            float d00 = Vector3.Dot(AB, AB);
            float d01 = Vector3.Dot(AB, AC);
            float d11 = Vector3.Dot(AC, AC);
            float d20 = Vector3.Dot(AP, AB);
            float d21 = Vector3.Dot(AP, AC);

            float denom = d00 * d11 - d01 * d01;
            float u = (d11 * d20 - d01 * d21) / denom;
            float v = (d00 * d21 - d01 * d20) / denom;

            int i = Mathf.Clamp((int)(u * 5f), 0, 4);
            int j = Mathf.Clamp((int)(v * 5f), 0, 4 - i);

            float fu = u * 5f - i;
            float fv = v * 5f - j;

            bool inverted = (fu + fv) > 1f;

            int index = 0;
            for (int ii = 0; ii < i; ii++)
                index += 2 * (5 - ii) - 1;

            index += 2 * j;
            if (inverted) index++;

            return index;
        }

        static Face FindFace(Vector3 v)
        {
            Face best = Faces[0];
            float bestDot = -1f;

            foreach (var f in Faces)
            {
                float d = Vector3.Dot(f.Normal, v);
                if (d > bestDot)
                {
                    bestDot = d;
                    best = f;
                }
            }
            return best;
        }

        static Vector3 RayPlaneHit(Vector3 dir, Face f)
        {
            float t = Vector3.Dot(f.A, f.Normal) / Vector3.Dot(dir, f.Normal);
            return dir * t;
        }

        static List<Face> BuildIcosahedron()
        {
            var v = new List<Vector3>();
            void add(float x, float y, float z) => v.Add(Vector3.Normalize(new Vector3(x, y, z)));

            add(0, 1, PHI); add(0, -1, PHI); add(0, 1, -PHI); add(0, -1, -PHI);
            add(1, PHI, 0); add(-1, PHI, 0); add(1, -PHI, 0); add(-1, -PHI, 0);
            add(PHI, 0, 1); add(-PHI, 0, 1); add(PHI, 0, -1); add(-PHI, 0, -1);

            int[][] faces = {
            new[]{0,1,8}, new[]{0,1,9}, new[]{0,4,5}, new[]{0,4,8}, new[]{0,5,9},
            new[]{1,6,7}, new[]{1,6,8}, new[]{1,7,9}, new[]{2,3,10}, new[]{2,3,11},
            new[]{2,4,5}, new[]{2,4,10}, new[]{2,5,11}, new[]{3,6,7}, new[]{3,6,10},
            new[]{3,7,11}, new[]{4,8,10}, new[]{5,9,11}, new[]{6,8,10}, new[]{7,9,11}
        };

            var result = new List<Face>();

            for (int i = 0; i < 20; i++)
            {
                Vector3 A = v[faces[i][0]];
                Vector3 B = v[faces[i][1]];
                Vector3 C = v[faces[i][2]];

                Vector3 n = Vector3.Normalize(Vector3.Cross(B - A, C - A));
                if (Vector3.Dot(n, (A + B + C) / 3f) < 0) n = -n;

                result.Add(new Face
                {
                    A = A,
                    B = B,
                    C = C,
                    Normal = n,
                    Letter = FaceLetters[i]
                });
            }

            return result;
        }
    }
}
