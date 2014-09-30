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
            {
                Console.WriteLine("Informar os projetos no arquivo texto.");
                Environment.Exit(0);
            }
                
            var lines = File.ReadAllLines(args[0]);
            var theBuild = new TheBuild();
            

            foreach (var item in lines)
            {
                theBuild.ProjectsPath.Add(item);
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Compilando projetos...\n\n");
            bool result = false;
            try
            {
                result = theBuild.Build();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                Environment.Exit(0);

            }
            
            if (!result)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\n");                
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
                Console.ForegroundColor = ConsoleColor.Gray;                
                Console.WriteLine("Removendo arquivos nao utilizados...\n");
                BuildFilesHandler.HandleFiles(lines[0]);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nFinalizado com sucesso!");
               
            }
            Console.WriteLine("\n\n");
            Console.ForegroundColor = ConsoleColor.White;
                

            
        }
    }
}
