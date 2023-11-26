window.JsInteropExerciseHelper = {
    blazorComponentRef: null,
    registerBlazorComponent(dotNetReference) {
        this.blazorComponentRef = dotNetReference;
    },

    getEmployees: function () {
        return [
            {id: 10, firstName: "First", lastName: "employee set by js", position: "Department of javascript studies"},
            {id: 20, firstName: "Second", lastName: "employee set by js", position: "Department of javascript studies"},
            {id: 30, firstName: "Third", lastName: "employee set by js", position: "Department of javascript studies"},
            {id: 40, firstName: "Fourth", lastName: "employee set by js", position: "Department of javascript studies"},
        ];
    },
    async callBlazorMethod() {
        if (this.blazorComponentRef) {
            await this.blazorComponentRef.invokeMethodAsync('SetValueFromJs',
                this.getEmployees()
            )
        } else {
            console.error('Blazor component reference not found.');
        }
    },

    isBlazorComponentRefInitialized() {
        return this.blazorComponentRef != null
    }
};