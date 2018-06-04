open System
open System.Text.RegularExpressions
open System.IO open System.Diagnostics
open System.Text.RegularExpressions
let searchPath = Environment.ExpandEnvironmentVariables "%USERPROFILE%\\nuget"
let scripts = Directory.EnumerateFiles(searchPath, "*.fsx", SearchOption.AllDirectories)
let asm =
    Directory.EnumerateDirectories(searchPath, "*lib*", SearchOption.AllDirectories)
    |> Seq.collect (fun d -> Directory.EnumerateFiles(d, "*.dll", SearchOption.AllDirectories))
    |> Seq.filter (Regex("FSharp.Core.dll").IsMatch>>not)

let args =
    seq [
        asm |> Seq.map (sprintf "-r:%s");
        scripts |> Seq.map (sprintf "--load:%s")
    ] |> Seq.concat |> String.concat " "

let bootstrap = new Process();
bootstrap.StartInfo <- new ProcessStartInfo("fsi.exe", args)
bootstrap.StartInfo.UseShellExecute <- true
bootstrap.Start() |> ignore