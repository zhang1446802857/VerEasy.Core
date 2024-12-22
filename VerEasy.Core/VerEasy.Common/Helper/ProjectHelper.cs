using System.Text;

namespace VerEasy.Common.Helper
{
    public static class ProjectHelper
    {
        /// <summary>
        /// 获取目录结构的静态方法
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="indent"></param>
        /// <returns></returns>
        public static string GetDirectoryStructure(string rootPath, string indent)
        {
            StringBuilder sb = new StringBuilder();

            // 获取当前目录下的所有文件和文件夹
            var items = Directory.GetFileSystemEntries(rootPath);

            int fileCount = 0; // 计数文件

            foreach (var item in items)
            {
                var fullPath = Path.Combine(rootPath, item);

                // 跳过某些目录：.vs, bin, debug, obj 等
                if (ShouldIgnoreDirectory(fullPath))
                {
                    continue;
                }

                if (Directory.Exists(fullPath))  // 如果是文件夹
                {
                    sb.AppendLine($"{indent}├── {Path.GetFileName(item)}/");  // 目录
                    sb.Append(GetDirectoryStructure(fullPath, indent + "│   "));  // 递归获取子目录
                }
                else  // 如果是文件
                {
                    // 只显示特定类型的文件
                    if (ShouldIncludeFile(item))
                    {
                        // 只显示第一个文件，其他用 "..." 表示
                        if (fileCount < 2)
                        {
                            sb.AppendLine($"{indent}├── {Path.GetFileName(item)}");  // 第一个文件
                        }
                        else if (fileCount == 2)
                        {
                            sb.AppendLine($"{indent}├── ...");  // 第二个文件，显示为 "..."
                        }
                        fileCount++;  // 增加文件计数
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 判断是否是需要忽略的目录
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        private static bool ShouldIgnoreDirectory(string fullPath)
        {
            // 忽略的目录列表：.vs, bin, debug, obj
            string[] ignoredDirs = { ".vs", "bin", "debug", "obj", "packages","Logs",
            "Properties",""};
            string folderName = Path.GetFileName(fullPath);

            // 使用 LINQ 的 Any 方法判断目录名是否在忽略列表中
            return ignoredDirs.Any(x => string.Equals(x, folderName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 需要过滤的文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static bool ShouldIncludeFile(string fileName)
        {
            // 定义需要排除的文件类型（扩展名）
            string[] excludedFileExtensions = { ".sln" };

            // 获取文件扩展名
            string fileExtension = Path.GetExtension(fileName)?.ToLower();

            // 判断文件扩展名是否不在排除的扩展名列表中
            return !excludedFileExtensions.Contains(fileExtension);
        }

        /// <summary>
        /// 生成txt格式
        /// </summary>
        public static void FileTxt()
        {
            // 请替换为你要生成目录结构的路径
            string rootPath = @"F:\DEV\ver-easy-core-3.0\VerEasy.Core";

            // 获取目录结构并生成带符号的树形 HTML
            string directoryStructure = GetDirectoryStructure(rootPath, "");

            // 将生成的结构写入到文件
            File.WriteAllText("directory_structure.txt", directoryStructure);
            Console.WriteLine("当前工作目录: " + Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// 生成html格式
        /// </summary>
        public static void FileHtml()
        {
            // 请替换为你要生成目录结构的路径
            string rootPath = @"F:\DEV\ver-easy-core-3.0\VerEasy.Core";

            // 获取目录结构并生成
            var directoryStructure = GetDirectoryStructure(rootPath);

            // 输出到 HTML 文件
            string outputFilePath = "index_with_structure.html";
            WriteToHtmlFile(outputFilePath, directoryStructure);

            Console.WriteLine("目录结构已生成并保存为 'index_with_structure.html'");
        }

        // 将目录结构写入到 HTML 文件
        private static void WriteToHtmlFile(string path, string directoryStructure)
        {
            try
            {
                // 生成完整的 HTML 内容
                StringBuilder htmlContent = new StringBuilder();
                htmlContent.AppendLine("<!DOCTYPE html>");
                htmlContent.AppendLine("<html lang=\"zh\">");
                htmlContent.AppendLine("<head>");
                htmlContent.AppendLine("<meta charset=\"UTF-8\">");
                htmlContent.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
                htmlContent.AppendLine("<meta name=\"author\" content=\"Mr.Z\">");
                htmlContent.AppendLine("<title>项目结构</title>");
                htmlContent.AppendLine("<style>");
                htmlContent.AppendLine("body { font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #f4f4f9; color: #333; }");
                htmlContent.AppendLine("ul { list-style-type: none; padding-left: 0; }");
                htmlContent.AppendLine("ul li { margin: 8px 0; }");
                htmlContent.AppendLine("ul li strong { color: #2d3a4e; font-weight: bold; }");
                htmlContent.AppendLine("</style>");
                htmlContent.AppendLine("</head>");
                htmlContent.AppendLine("<body>");
                htmlContent.AppendLine("<header><h1>项目结构</h1></header>");
                htmlContent.AppendLine("<section><div><h2>项目目录结构</h2>");

                // 将目录结构嵌入到 HTML 中
                htmlContent.AppendLine(directoryStructure);

                htmlContent.AppendLine("</div></section>");
                htmlContent.AppendLine("</body>");
                htmlContent.AppendLine("</html>");

                // 写入 HTML 文件
                File.WriteAllText(path, htmlContent.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"写入 HTML 文件时出错: {ex.Message}");
            }
        }

        // 获取目录结构并生成树状 HTML
        public static string GetDirectoryStructure(string rootPath)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<ul>");

            // 获取当前目录下的所有文件和文件夹
            var items = Directory.GetFileSystemEntries(rootPath);

            foreach (var item in items)
            {
                var fullPath = Path.Combine(rootPath, item);

                // 忽略不需要的目录
                if (ShouldIgnoreDirectory(fullPath)) continue;

                if (Directory.Exists(fullPath))  // 如果是文件夹
                {
                    sb.AppendLine($"<li><strong>{Path.GetFileName(item)}</strong>");
                    sb.AppendLine("<ul>");
                    sb.Append(GetDirectoryStructure(fullPath));  // 递归获取子目录结构
                    sb.AppendLine("</ul>");
                    sb.AppendLine("</li>");
                }
                else  // 如果是文件
                {
                    if (ShouldIncludeFile(item))  // 根据文件类型过滤
                    {
                        sb.AppendLine($"<li>{Path.GetFileName(item)}</li>");
                    }
                }
            }

            sb.AppendLine("</ul>");
            return sb.ToString();
        }
    }
}