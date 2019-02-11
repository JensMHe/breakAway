
// REMOVE ADDRESS
$('body').on('click', '.deleteBtn', function () {
    $(this).parent().parent().remove();
});

// ADD ADDRESS
$('.addBtn').on('click', function () {

    if (($('#newStreet1').val() != '' || $('#newStreet2').val() != '' || $('#newCity').val() != '') && $('#newAddressType').val() != '') {

        var index = parseInt($('tr.address').last().find('#addresses_Index').val()) + 1;
        if (isNaN(index)) {
            index = 0;
        }

        var newAddressItem = $('#newAddressItem').clone();

        newAddressItem.attr('class', 'address').removeAttr('id');
        newAddressItem.find('button').attr('class', 'btn btn-danger deleteBtn').html('Delete');

        var indexName = "addresses.Index";
        var idName = "Addresses[0].Id".replace('0', index);
        var street1Name = "Addresses[0].Street1".replace('0', index);
        var street2Name = "Addresses[0].Street2".replace('0', index);
        var cityName = "Addresses[0].City".replace('0', index);
        var addressTypeName = "Addresses[0].AddressType".replace('0', index);
        var addressTypeId = "address[0]Id".replace('0', index);

        newAddressItem.find('#newIndex').attr('value', index).attr('name', indexName).attr('id', 'addresses_Index');
        newAddressItem.find('#newId').attr('value', '0').attr('name', idName).removeAttr('id');
        newAddressItem.find('#newStreet1').attr('name', street1Name).removeAttr('id');
        newAddressItem.find('#newStreet2').attr('name', street2Name).removeAttr('id');
        newAddressItem.find('#newCity').attr('name', cityName).removeAttr('id');
        newAddressItem.find('#newAddressType').addClass('addressTypeListItem').attr('name', addressTypeName).removeAttr('id');

        $('#addressList').find('tbody').append(newAddressItem);

        $('#newAddressItem').find('input').val('');
    }
});

// SAVE ADDRESSES
$(document).ready(function () {
    $('.addressTypeListItem').keyup(function () {
        var empty = false;
        $('.addressTypeListItem').each(function () {
            if ($(this).val().replace(/^\s+|\s+$/g, "").length == 0) {
                empty = true;
                $(this).attr('style', 'border:dashed; border-color:red; border-width:2px').attr('placeholder', 'Address type is required');
            }
            else {
                $(this).removeAttr('style');
            }
        });

        if (empty) {
            $('#saveBtn').attr('disabled', 'disabled');
        } else {
            $('#saveBtn').removeAttr('disabled');
        }
    });
});