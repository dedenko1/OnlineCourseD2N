window.camera = {
    start: async () => {
        const video = document.getElementById("cameraVideo");

        const stream = await navigator.mediaDevices.getUserMedia({
            video: true
        });

        video.srcObject = stream;
        await video.play();
    },

    capture: () => {
        const video = document.getElementById("cameraVideo");
        const canvas = document.createElement("canvas");

        canvas.width = video.videoWidth;
        canvas.height = video.videoHeight;

        const ctx = canvas.getContext("2d");
        ctx.drawImage(video, 0, 0);

        return canvas.toDataURL("image/jpeg");
    },

    stop: () => {
        const video = document.getElementById("cameraVideo");
        const stream = video.srcObject;

        if (stream) stream.getTracks().forEach(t => t.stop());

        video.srcObject = null;
    }
};
