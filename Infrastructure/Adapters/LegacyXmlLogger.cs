// O Sistema Legado (Simulado - finge que é uma DLL antiga que você não pode mudar)
namespace Infrastructure.LegacySystem
{
    public class LegacyXmlLogger
    {
        public void WriteXmlLog(string xmlContent)
        {
            // Simula gravação em disco ou sistema antigo
            Console.WriteLine($"[SISTEMA LEGADO] Gravando XML: {xmlContent}");
        }
    }
}