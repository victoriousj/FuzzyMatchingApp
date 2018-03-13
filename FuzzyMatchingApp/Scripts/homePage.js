$(document).ready(function () {
    $('.name-picker').select2({
        //placeholder: 'Search for a Customer',
        minimumInputLength: 4,
        ajax: {
            url: '/Api/CustomersSearch',
            dataType: 'json',
            delay: 500,
            processResults: function (data, params) {
                return {
                    results: $.map(data, function (result) {
                        return { id: result.id, text: (result.firstName + ' ' + result.lastName)}
                    })
                }
            }
        }
    });

    $('.name-picker').on("select2:select", function (e) {
        var customerId = $('.name-picker').val();
        $.ajax('/Api/GetCustomerAddress/' + customerId)
            .done(function (e) {
                $('#customer').html('Name: ' + e.lastName + ', ' + e.firstName+ '<br /> Phone: ' + e.phoneNumber + '<br />Address:' + e.address);
        })
    });
});