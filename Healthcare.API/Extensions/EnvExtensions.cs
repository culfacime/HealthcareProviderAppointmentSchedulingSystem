using DotNetEnv;

namespace Healthcare.API.Extensions
{
    public class EnvExtensions
    {
        public static void LoadEnv()
        {
            var path = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (path.Parent != null && !File.Exists(Path.Join(path.FullName, ".env")))
                path = path.Parent;

            if (path.Exists)
                Env.Load(Path.Join(path.FullName, ".env"));
            else
                Env.Load();
        }
    }
}
