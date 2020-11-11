namespace JacobDixon.AspNetCore.LiveSassCompile.Compilers
{
    public interface ICompiler
    {
        void Compile(string path);
        bool IsExcluded(string fileName);
    }
}