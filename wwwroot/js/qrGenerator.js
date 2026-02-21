/**
 * Simple wrapper for QR Code generation using qrcode.js
 */
window.qrGenerator = {
    generate: function (elementId, text, size = 128) {
        const container = document.getElementById(elementId);
        if (!container) return;

        container.innerHTML = ""; // Clear
        new QRCode(container, {
            text: text,
            width: size,
            height: size,
            colorDark: "#000000",
            colorLight: "#ffffff",
            correctLevel: QRCode.CorrectLevel.H
        });
    }
};
