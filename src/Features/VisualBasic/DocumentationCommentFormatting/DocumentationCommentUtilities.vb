' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Namespace Microsoft.CodeAnalysis.VisualBasic.DocumentationCommentFormatting
    Friend Class DocumentationCommentUtilities
        Private Shared ReadOnly newLineStrings As String() = {vbCrLf, vbCr, vbLf}

        Public Shared Function ExtractXMLFragment(docComment As String) As String
            Dim splitLines = docComment.Split(newLineStrings, StringSplitOptions.None)

            For i = 0 To splitLines.Length - 1
                If splitLines(i).StartsWith("'''") Then
                    splitLines(i) = splitLines(i).Substring(3)
                End If
            Next

            Return splitLines.Join(vbCrLf)
        End Function
    End Class
End Namespace
