using JacobDixon.AspNetCore.LiveSassCompile.Compilers.Tests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LiveSassCompileUnitTests.Compilers
{
    public class SassCompilerTestDataCollection : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var sourceDirectory = Path.Combine(TestPaths.SourceDirectory, "SassCompilerTests");
            var destinationDirectory = Path.Combine(TestPaths.DestinationDirectory, "SassCompilerTests");

            // Single top level file compile
            yield return new object[] {
                new List<SassCompilerTestData> {
                    new SassCompilerTestData {
                        SourceLocation = Path.Combine(sourceDirectory, "styles.scss"),
                        DestinationLocation = Path.Combine(destinationDirectory, "styles.css")
                    }
                },
                sourceDirectory,
                destinationDirectory
            };
            // Single file in sub directory compile
            yield return new object[] {
                new List<SassCompilerTestData> {
                    new SassCompilerTestData {
                        SourceLocation = Path.Combine(sourceDirectory, "subfolder", "styles.scss"),
                        DestinationLocation = Path.Combine(destinationDirectory, "subfolder", "styles.css")
                    }
                },
                sourceDirectory,
                destinationDirectory
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class SassCompilerTestData
    {
        public string SourceLocation { get; set; }
        public string FileContent { get; set; } = @"body {  
color: red;
}";
        public string DestinationLocation { get; set; }
    }
}
