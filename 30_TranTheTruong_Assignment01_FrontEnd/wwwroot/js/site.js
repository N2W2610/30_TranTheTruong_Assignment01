// Fills a modal form from the data-* attributes of the clicked button.
// Form fields are matched by their name (case-insensitive), e.g. name="AccountName" <- data-accountname.
(function () {
    function fillForm(form, data) {
        form.reset();
        Array.from(form.elements).forEach(function (el) {
            if (!el.name) return;
            var key = el.name.toLowerCase();
            if (!(key in data)) return;
            var value = data[key];

            if (el.tagName === 'SELECT' && el.multiple) {
                var ids = value ? value.split(',') : [];
                Array.from(el.options).forEach(function (o) { o.selected = ids.indexOf(o.value) >= 0; });
            } else {
                el.value = value;
            }
        });
    }

    document.addEventListener('click', function (e) {
        var edit = e.target.closest('.btn-edit');
        if (edit) {
            var modalEl = document.querySelector(edit.dataset.modal);
            fillForm(modalEl.querySelector('form'), edit.dataset);
            bootstrap.Modal.getOrCreateInstance(modalEl).show();
            return;
        }

        var del = e.target.closest('.btn-delete');
        if (del) {
            var delModal = document.querySelector(del.dataset.modal);
            delModal.querySelector('input[name="id"]').value = del.dataset.id;
            delModal.querySelector('.delete-label').textContent = del.dataset.label || del.dataset.id;
            bootstrap.Modal.getOrCreateInstance(delModal).show();
        }
    });
})();
