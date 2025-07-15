window.playClickSound = () => {
    const audio = new Audio('/sounds/click.mp3');
    audio.play();
};

let testInstance = null;

window.registerTestInstance = (dotNetObj) => {
    testInstance = dotNetObj;
};

window.startTestTimer = function (durationMinutes) {
    function tryStartTimer() {
        const display = document.getElementById('test-timer');
        if (!display) {
            setTimeout(tryStartTimer, 500);
            return;
        }

        let timeLeft = durationMinutes * 60;

        const timerInterval = setInterval(() => {
            const minutes = Math.floor(timeLeft / 60);
            const seconds = timeLeft % 60;
            display.textContent = `${minutes}:${seconds.toString().padStart(2, '0')}`;

            if (--timeLeft < 0) {
                clearInterval(timerInterval);
                alert("⏰ انتهى الوقت!");
                if (testInstance) {
                    testInstance.invokeMethodAsync("SubmitFromTimer");
                }
            }
        }, 1000);
    }

    tryStartTimer();
};

window.previewImageFromInput = (inputId, imgId) => {
    const input = document.getElementById(inputId);
    const img = document.getElementById(imgId);
    const file = input.files[0];

    if (file) {
        const url = URL.createObjectURL(file);
        img.src = url;
    }
};
