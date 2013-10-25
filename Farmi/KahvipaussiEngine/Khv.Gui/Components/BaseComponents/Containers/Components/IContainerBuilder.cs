using Khv.Gui.Components.BaseComponents.Containers.Collections;

namespace Khv.Gui.Components.BaseComponents.Containers.Components
{
    /// <summary>
    /// Rajapinta joka tulee periä ja passata ContainerManagerille
    /// kun luodaan uutta ikkuna setuppia. 
    /// </summary>
    /// <typeparam name="T">Container tyyppi joka perii pohjaluokan Container.</typeparam>
    public interface IContainerBuilder<T> where T : Container
    {
        T BuildMainContainer(ContainerManager<T> containerManager);
        T[] BuildChildContainers(ContainerManager<T> containerManager);
    }
}
