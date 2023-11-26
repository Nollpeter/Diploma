window.JsToBlazor = {
    blazorComponentRef: null,
    registerBlazorComponent(dotNetReference) {
        this.blazorComponentRef = dotNetReference;
    },

    async callBlazorMethod()  {
        if (this.blazorComponentRef) {
            await this.blazorComponentRef.invokeMethodAsync('SetValueFromJs', {id: 10, name: "Test name set by js"})
            await this.blazorComponentRef.invokeMethodAsync('SetValueFromJsAsync', {id: 20, name: "Test name 2 set by js"})
        } else {
            console.error('Blazor component reference not found.');
        }
    }
};