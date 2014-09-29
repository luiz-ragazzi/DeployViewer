using Financial.Builder;
using Financial.Builder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeployViewer
{
    class Program
    {
        static void Main(string[] args)
        {


            Console.ForegroundColor = ConsoleColor.Yellow;
            if (args.Count() == 0 || !File.Exists(args[0]))
                throw new NullReferenceException("Informar os projetos no arquivo texto.");

            var lines = File.ReadAllLines(args[0]);
            var theBuild = new TheBuild();
            
            foreach (var item in lines)
            {
                theBuild.ProjectsPath.Add(item);
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Compilando projetos...\n\n");

            bool result = theBuild.Build();
            if (!result)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n");
                Console.WriteLine("\n");
                Console.WriteLine("Erro de compilação:\n");
               
                Console.ForegroundColor = ConsoleColor.Cyan;
                foreach (var projeto in theBuild.Errors)
                {
                    Console.WriteLine(">>>>>>>>>>>>>{0}", projeto);
                }
            }
            else
            {
                
                //Pegar o diretorio dos projetos
                var directoryProject = Directory.GetParent(Path.GetDirectoryName(lines[0]));

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Projetos compilados com sucesso!\n\n\n");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Gerando build...\n\n");
                
                Process proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = @"C:\Windows\Microsoft.NET\Framework\v2.0.50727\aspnet_compiler.exe",
                        Arguments = string.Format(@"-v /Financial.Web -p {0}\Financial.Web -u -f -c -d {0}\Deploy", directoryProject),
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = false
                    }
                };

                Process proc2 = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = @"C:\Program Files (x86)\msbuild\Microsoft\WebDeployment\v8.0\aspnet_merge.exe",
                        Arguments = string.Format(@"{0}\Deploy  -o Financial.Web_Deploy -copyattrs", directoryProject),
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                proc.Start();
                while (!proc.StandardOutput.EndOfStream)
                {
                    proc.StandardOutput.ReadLine();
                   
                    
                }
                
                proc2.Start();
                while (!proc2.StandardOutput.EndOfStream)
                {
                    proc2.StandardOutput.ReadLine();
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Build gerado com sucesso!\n\n\n");

                Console.ForegroundColor = ConsoleColor.Gray;
                //Remove Arquivos
                Console.WriteLine("Removendo arquivos...\n");
                BuildFilesHandler.HandleFiles(lines[0]);

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nFinalizado com sucesso!");
               
            }
            Console.WriteLine("\n\n");
            Console.ForegroundColor = ConsoleColor.White;
                

            
        }
    }
}
