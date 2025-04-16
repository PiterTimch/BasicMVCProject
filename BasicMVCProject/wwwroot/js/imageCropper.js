let input, image, croppedDataInput, cropper, form, cropContainer;

document.addEventListener("DOMContentLoaded", function () {
    console.log("DOM fully loaded and parsed");
    input = document.getElementById("ImageFile");
    image = document.getElementById("croppingImage");
    croppedDataInput = document.getElementById("croppedImageData");
    form = document.querySelector("form");
    cropContainer = document.getElementById("imageCropContainer");

    input.addEventListener("change", function () {
        const file = this.files[0];
        if (file && /^image\/\w+/.test(file.type)) {
            const url = URL.createObjectURL(file);
            image.src = url;
            cropContainer.style.display = "block";
            image.style.display = "block";

            if (cropper) cropper.destroy();

            cropper = new Cropper(image, {
                aspectRatio: 16 / 9,
                viewMode: 1,
                autoCropArea: 1,
                cropend: function () {
                    const canvas = cropper.getCroppedCanvas();
                    croppedDataInput.value = canvas.toDataURL("image/webp");
                }
            });
        }
    });

    form.addEventListener("submit", function () {
        if (cropper) {
            const canvas = cropper.getCroppedCanvas();
            croppedDataInput.value = canvas.toDataURL("image/webp");
        }
    });
});
