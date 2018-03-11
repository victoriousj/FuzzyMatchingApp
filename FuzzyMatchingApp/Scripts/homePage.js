$(document).ready(function () {
    $('.name-picker').select2({
        //placeholder: 'Search for a Customer',
        minimumInputLength: 4,
        ajax: {
            url: '/Api/CustomersSearch',
            dataType: 'json',
            quietMillis: 2000,
            processResults: function (data, params) {
                return {
                    results: $.map(data, function (result) {
                        return { id: result.ID, text: (result.FirstName + ' ' + result.LastName)}
                    })
                }
            }
        }
    });
});