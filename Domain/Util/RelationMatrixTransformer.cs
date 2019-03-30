using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Util
{
    public class RelationMatrixTransformer
    {
        public static int[,] GetTransformedRelationMatrix(string[,] relationMatrix)
        {
            int size = relationMatrix.GetLength(0);
            int[,] numericRelationMatrix = new int[size, size];

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (relationMatrix[i, j] != "")
                        if (relationMatrix[i, j] == "<")
                            numericRelationMatrix[i, j] = 1;
                        else if (relationMatrix[i, j] == "=")
                            numericRelationMatrix[i, j] = 2;
                        else if (relationMatrix[i, j] == ">")
                            numericRelationMatrix[i, j] = 3;
                        else numericRelationMatrix[i, j] = 0;

            return numericRelationMatrix;
        }

    }
}
