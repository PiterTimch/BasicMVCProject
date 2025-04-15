const deleteButtons = document.querySelectorAll('.btn-danger[data-bs-toggle="modal"]');

deleteButtons.forEach(button => {
button.addEventListener('click', function () {
    const categoryId = this.getAttribute('data-id');

    const confirmDeleteBtn = document.getElementById('confirmDeleteBtn');

    confirmDeleteBtn.setAttribute('asp-route-id', categoryId);
});
});
