window.cameraInterop = {
    takePhoto: async function () {
        return await navigator.mediaDevices.getUserMedia({ video: true });
    }
};