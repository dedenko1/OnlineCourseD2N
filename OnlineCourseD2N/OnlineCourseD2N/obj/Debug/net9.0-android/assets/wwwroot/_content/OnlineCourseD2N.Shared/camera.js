window.cameraInterop = {
    stream: null,
    dotNetReference: null,

    // Mulai kamera dan kirim status ke .NET
    startCapture: async function (dotNetRef, videoElementId = "cameraVideoElement") {
        this.dotNetReference = dotNetRef;

        const video = document.getElementById(videoElementId);
        if (!video) {
            console.error(`Video element '${videoElementId}' not found.`);
            if (dotNetRef) await dotNetRef.invokeMethodAsync('CaptureCancelled');
            return false;
        }

        try {
            const constraints = { video: { facingMode: "environment" }, audio: false };
            this.stream = await navigator.mediaDevices.getUserMedia(constraints);
            video.srcObject = this.stream;
            await video.play();
            return true;
        } catch (err) {
            console.error("Error accessing camera:", err);
            if (dotNetRef) await dotNetRef.invokeMethodAsync('CaptureCancelled');
            return false;
        }
    },

    // Ambil foto, kembalikan base64, tapi kamera tetap aktif
    capturePhoto: function (videoElementId = "cameraVideoElement") {
        if (!this.stream) {
            console.warn("No active camera stream.");
            return null;
        }

        const video = document.getElementById(videoElementId);
        if (!video) return null;

        const canvas = document.createElement("canvas");
        canvas.width = video.videoWidth || 640;
        canvas.height = video.videoHeight || 480;

        const context = canvas.getContext("2d");
        if (!context) return null;

        context.drawImage(video, 0, 0, canvas.width, canvas.height);
        const base64 = canvas.toDataURL("image/png");

        if (this.dotNetReference) {
            this.dotNetReference.invokeMethodAsync('PhotoCaptured', base64);
        }

        return base64;
    },

    // Hentikan kamera
    stopCamera: function (videoElementId = "cameraVideoElement") {
        if (this.stream) {
            this.stream.getTracks().forEach(track => track.stop());
            this.stream = null;
        }

        const video = document.getElementById(videoElementId);
        if (video) video.srcObject = null;
    }
};
