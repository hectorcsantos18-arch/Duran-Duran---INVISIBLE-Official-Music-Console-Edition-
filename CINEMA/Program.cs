using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

class Program
{
    // Definição das frequências para a nossa linha de sintetizador (Synth-bass de Invisible)
    const double Bass_Mi = 165.0;  // E3
    const double Bass_Sol = 196.0; // G3
    const double Bass_La = 220.0;  // A3
    const double Bass_Si = 247.0;  // B3
    const int Bass_Do = 261;  // C4
    const int Bass_Re = 294;  // D4

    const uint SND_FILENAME = 0x00020000;
    const uint SND_ASYNC = 0x0001;
    const uint SND_NODEFAULT = 0x0002;

    [DllImport("winmm.dll", EntryPoint = "PlaySound", SetLastError = true, CharSet = CharSet.Unicode)]
    static extern bool PlaySound(string pszSound, IntPtr hmod, uint fdwSound);

    static void Main()
    {
        Console.Title = "Duran Duran - INVISIBLE (Official Music Console Edition)";

        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear();

        ExibirTextoColorido("==================================================\n", ConsoleColor.DarkGray);
        ExibirTextoColorido("      DURAN DURAN - INVISIBLE (Audio Corrigido)   \n", ConsoleColor.Magenta);
        ExibirTextoColorido("==================================================\n\n", ConsoleColor.DarkGray);

        Console.WriteLine("Aumente o som e pressione qualquer tecla para iniciar...");
        Console.ReadKey();
        Console.Clear();

        // ------------------ VERSO 1 ------------------
        Console.ForegroundColor = ConsoleColor.Gray;
        TocarNotaELetra(Bass_Mi, 250, "Walking by the wall...\n");
        TocarNotaELetra(Bass_Mi, 250, "(Shy one) the shadows will not fall\n");
        TocarNotaELetra(Bass_Sol, 250, "(Shy one) is silently ignored\n");
        TocarNotaELetra(Bass_La, 400, "(Quiet one) discouraged by the noise\n\n");
        Thread.Sleep(600);

        TocarNotaELetra(Bass_Mi, 250, "(Quiet one) living without choice\n");
        TocarNotaELetra(Bass_Sol, 250, "(Quiet one) is a life without a voice\n\n");
        Thread.Sleep(800);

        // ------------------ REFRÃO COLORIDO ------------------
        Console.ForegroundColor = ConsoleColor.Cyan;

        Console.WriteLine("--- [ REFRÃO ] ---");
        TocarNotaELetra(Bass_Si, 300, "When you can't even say my name\n");
        TocarNotaELetra(Bass_Do, 300, "Has the memory gone? Are you feeling numb?\n");
        TocarNotaELetra(Bass_Re, 300, "Go and call my name\n");
        TocarNotaELetra(Bass_Do, 300, "I can't play this game, so I ask again\n");
        TocarNotaELetra(Bass_Si, 300, "Will you say my name?\n");
        TocarNotaELetra(Bass_La, 300, "Has the memory gone? Are you feeling numb?\n");

        Console.ForegroundColor = ConsoleColor.Red;
        TocarNotaELetra(Bass_Mi, 600, "Or have I become INVISIBLE?\n\n");
        Thread.Sleep(1000);

        // ------------------ VERSO 2 ------------------
        Console.ForegroundColor = ConsoleColor.Gray;
        TocarNotaELetra(Bass_Mi, 250, "A dream that dreamers wish away...\n");
        TocarNotaELetra(Bass_Sol, 250, "(Hindsight) is falling on my face\n");
        TocarNotaELetra(Bass_La, 400, "(Highlights) the shape of my disgrace\n\n");
        Thread.Sleep(600);

        // ------------------ REFRÃO COLORIDO 2 ------------------
        Console.ForegroundColor = ConsoleColor.Cyan;

        Console.WriteLine("--- [ REFRÃO ] ---");
        TocarNotaELetra(Bass_Si, 300, "When you don't hear a word I say\n");
        TocarNotaELetra(Bass_Do, 300, "As the talking goes, it's a one-way flow\n");
        TocarNotaELetra(Bass_Re, 300, "No fault, no blame\n");
        TocarNotaELetra(Bass_Do, 300, "Has the memory gone? Are you feeling numb?\n");

        Console.ForegroundColor = ConsoleColor.Red;
        TocarNotaELetra(Bass_Mi, 600, "And have I become INVISIBLE?\n\n");
        Thread.Sleep(1000);

        // ------------------ PONTE E FINAL ------------------
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        TocarNotaELetra(Bass_La, 300, "And no one hears a word they say\n");
        TocarNotaELetra(Bass_Si, 300, "Has the memory gone? Are you feeling numb?\n");
        TocarNotaELetra(Bass_Do, 300, "Not a word they say\n");
        TocarNotaELetra(Bass_Re, 300, "But a voiceless crowd isn't backing down\n");
        TocarNotaELetra(Bass_Mi, 350, "When the air turns red with their loaded hesitation...\n\n");
        Thread.Sleep(500);

        // REFRÃO FINAL
        Console.ForegroundColor = ConsoleColor.Cyan;
        TocarNotaELetra(Bass_Si, 300, "Can you say my name?\n");
        TocarNotaELetra(Bass_Do, 300, "Has the memory gone? Are you feeling numb?\n");

        Console.ForegroundColor = ConsoleColor.Red;
        TocarNotaELetra(Bass_Mi, 800, "Have we all become INVISIBLE?\n");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\n[Fade out - Sintetizador finalizando]");
        GerarSomSintetizado(Bass_Mi, 400);
        GerarSomSintetizado(Bass_Sol, 400);
        GerarSomSintetizado(Bass_Mi, 800);

        Console.ResetColor();
        Console.WriteLine("\n\n✨ Fim da reprodução! Pressione qualquer tecla para fechar.");
        Console.ReadKey();
    }

    static void TocarNotaELetra(double frequencia, int duracao, string texto)
    {
        Console.Write(texto);
        GerarSomSintetizado(frequencia, duracao);
        Thread.Sleep(50);
    }

    static void GerarSomSintetizado(double frequencia, int duracaoMs)
    {
        if (frequencia <= 0 || duracaoMs <= 0)
            return;

        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            try
            {
                var caminho = Path.Combine(Path.GetTempPath(), $"cinema-tone-{Guid.NewGuid():N}.wav");
                CriarArquivoWav(caminho, frequencia, duracaoMs, 1.2);
                PlaySound(caminho, IntPtr.Zero, SND_FILENAME | SND_ASYNC | SND_NODEFAULT);
                Thread.Sleep(duracaoMs + 50);
                File.Delete(caminho);
            }
            catch
            {
                try
                {
                    if (OperatingSystem.IsWindows())
                    {
                        Console.Beep((int)Math.Clamp(frequencia, 37, 32767), duracaoMs);
                    }
                    else
                    {
                        Console.Write("\a");
                        Thread.Sleep(duracaoMs);
                    }
                }
                catch
                {
                    Console.Write("\a");
                    Thread.Sleep(duracaoMs);
                }
            }
        }
        else
        {
            Console.Write("\a");
            Thread.Sleep(duracaoMs);
        }
    }

    static void CriarArquivoWav(string caminho, double frequencia, int duracaoMs, double volume)
    {
        const int sampleRate = 44100;
        const int canais = 1;
        const int bitsPorSample = 16;

        int totalSamples = (int)(sampleRate * (duracaoMs / 1000.0));
        int dataSize = totalSamples * (bitsPorSample / 8) * canais;
        int byteRate = sampleRate * canais * (bitsPorSample / 8);
        int blockAlign = canais * (bitsPorSample / 8);

        using var stream = new FileStream(caminho, FileMode.Create, FileAccess.Write, FileShare.None);
        using var writer = new BinaryWriter(stream);

        writer.Write(new byte[] { (byte)'R', (byte)'I', (byte)'F', (byte)'F' });
        writer.Write(36 + dataSize);
        writer.Write(new byte[] { (byte)'W', (byte)'A', (byte)'V', (byte)'E' });
        writer.Write(new byte[] { (byte)'f', (byte)'m', (byte)'t', (byte)' ' });
        writer.Write(16);
        writer.Write((short)1);
        writer.Write((short)canais);
        writer.Write(sampleRate);
        writer.Write(byteRate);
        writer.Write((short)blockAlign);
        writer.Write((short)bitsPorSample);
        writer.Write(new byte[] { (byte)'d', (byte)'a', (byte)'t', (byte)'a' });
        writer.Write(dataSize);

        for (int i = 0; i < totalSamples; i++)
        {
            double tempo = i / (double)sampleRate;
            double seno = Math.Sin(2 * Math.PI * frequencia * tempo);
            double ganho = Math.Clamp(seno * volume, -1.0, 1.0);
            short sample = (short)(ganho * short.MaxValue * 0.95);
            writer.Write(sample);
        }
    }

    static void ExibirTextoColorido(string texto, ConsoleColor cor)
    {
        Console.ForegroundColor = cor;
        Console.Write(texto);
        Console.ResetColor();
    }
}
