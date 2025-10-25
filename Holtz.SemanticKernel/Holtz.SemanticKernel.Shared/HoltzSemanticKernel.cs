using Holtz.SemanticKernel.Shared.Enums;
using Microsoft.SemanticKernel;

namespace Holtz.SemanticKernel.Shared
{
    public static class HoltzSemanticKernel
    {
        public static Kernel CreateSemanticKernel(EnumSemanticKernelType type)
        {
            switch (type)
            {
                case EnumSemanticKernelType.SimpleKernel:
                    return Kernel.CreateBuilder().Build();
                default: throw new ArgumentException($"Type {type} not implemented!");
            }
        }
    }
}
