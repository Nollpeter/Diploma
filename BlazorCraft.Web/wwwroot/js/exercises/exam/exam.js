window.ExamHelper= {
    async fetchRandomPersonImage(dotnetHelper, gender){
        const randomNumber = Math.floor(Math.random() * 25) + 1;
        const genderFolder = typeof gender === 'number' ? (gender === 1 ? 'male' : 'female') : (gender === 'male' ? 'male' : 'female');
        // Set the url to fetch from disk
        const url = `/img/profile-pictures/${genderFolder}/${randomNumber}.jpeg`;
        fetch(url).then(response => {
            response.blob().then(blob => {
                let reader = new FileReader();
                reader.onloadend = function () {
                    dotnetHelper.invokeMethodAsync('UpdateImage', reader.result);
                }
                reader.readAsDataURL(blob);
            });
        });
    }

}