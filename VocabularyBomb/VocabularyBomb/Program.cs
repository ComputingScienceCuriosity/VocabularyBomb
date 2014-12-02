#region File Description
#endregion

using System;

namespace Vocabulary
{
    static class Program
    {
        
        static void Main(string[] args)
        {
           
            using (VocabularyBomb game = new VocabularyBomb())
            {
                    game.Run();
            }
        
            
        }
    }
}

