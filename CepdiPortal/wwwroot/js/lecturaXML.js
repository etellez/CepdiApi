$(document).ready(function () {

    const token = localStorage.getItem('token');
    if (!token) {
        window.location.href = '/Home/Index';
    }

    Dropzone.options.xmlDropzone = {
        paramName: "file", // Nombre del parámetro esperado en el servidor
        maxFiles: 1, // Permitir solo un archivo
        acceptedFiles: ".xml", // Aceptar solo archivos XML
        init: function () {
            this.on("success", function (file, response) {
                // Mostrar el PDF en una nueva ventana
                const pdfWindow = window.open("");
                pdfWindow.document.write(`
                    <iframe width="100%" height="100%" src="data:application/pdf;base64,${response.pdfBase64}"></iframe>
                `);
            });
            this.on("error", function (file, response) {
                alert(`Error al procesar el archivo: ${response}`);
            });
        }
    };
});
