window.ExamHelper= {
    async fetchRandomPersonImage(dotnetHelper){
        
        const targetUrl = 'https://thispersondoesnotexist.com';
        const url = 'https://corsproxy.io/?' + encodeURIComponent(targetUrl);
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