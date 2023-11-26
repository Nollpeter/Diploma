window.ExamHelper= {
    async fetchRandomPersonImage(dotnetHelper){

        const url = 'https://thispersondoesnotexist.com/image';
        const response = await fetch(url, { method: 'GET', mode: 'cors' });
        const blob = await response.blob();
        const reader = new FileReader();
        reader.readAsDataURL(blob);
        reader.onloadend = function () {
            var base64data = reader.result;
            dotnetHelper.invokeMethodAsync('UpdateImage', base64data);
        }
    }

}