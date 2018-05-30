var bookCatalog = bookCatalog || {};

(function () {
    var self = this;
    var bookTable;
    var bookTableEditor;
    var urlForTable = '';
    var urlForUpdateBook = '';

    self.setUrlForTable = function (url) {
        urlForTable = url;
    };

    self.InitBookTable = function () {
        bookTable = $('#BookCatalogTable').dataTable({
            "bSort": true,
            "bAutoWidth": true,
            "aaSorting": [[1, "asc"]],
            "bPaginate": true,
            "sPaginationType": "first_last_numbers",
            "iDisplayLength": 2,
            "bFilter": false,
            "bInfo": false,
            "bServerSide": true,
            "sAjaxSource": urlForTable,
            "columnDefs": [{
                "targets": [2, 6, 7, 8],
                "orderable": false
            }],
            "processing": true,
            "language": {
                "processing": '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i>'
            },
            "aoColumns": [
                {
                    "className": 'details-control',
                    "orderable": false,
                    "data": null,
                    "defaultContent": '',
                    "render": function () {
                        return '<i class="fa fa-plus-square" aria-hidden="true"></i>';
                    },
                    width: "15px"
                },
                { "data": "Id" },
                { "data": "Title" },
                { "data": "PublishingYear" },
                { "data": "PagesAmount" },
                { "data": "Rate" },
                { "data": "Authors" },
                {
                    "className": 'edit-control',
                    "render": function (data, type, row, meta) {
                        return '<i id="editBookButton" class="fa fa-edit"></i>';
                    }
                },
                {
                    "className": 'remove-control',
                    "render": function (data, type, row, meta) {
                        return '<i id="removeBookButton" class="fa fa-trash"></i>';
                    }
                }
            ],
            "fnServerData": function (sSource, aoData, fnCallback, oSettings) {
                oSettings.jqXHR = $.ajax({
                    "dataType": 'json',
                    "type": "POST",
                    "url": sSource,
                    "data": aoData,
                    "success": function (json) {
                        fnCallback(json);
                    }
                })
            }
        });

        $('#BookCatalogTable tbody').on('click', 'td.details-control', function () {
            var tableRow = $(this).closest('tr');
            var tdi = tableRow.find("i.fa");
            var row = bookTable.api().row(tableRow);

            if (row.child.isShown()) {
                row.child.hide();
                tableRow.removeClass('shown');
                tdi.first().removeClass('fa-minus-square');
                tdi.first().addClass('fa-plus-square');
            }
            else {
                row.child(format(row.data())).show();
                tableRow.addClass('shown');
                tdi.first().removeClass('fa-plus-square');
                tdi.first().addClass('fa-minus-square');
            }
        });

        $('#BookCatalogTable tbody').on('click', 'td.edit-control', function () {
            return '<div id="editModal" class="modal">' +
                '<div class="modal-content">' +
                '<p>Some text in modal</p>' +
                '</div></div >';
        });

        bookTable.on('user-select', function (e, dt, type, cell, originalEvent) {
            if ($(cell.node()).hasClass("details-control")) {
                e.preventDefault();
            }
        });
    };

    self.initEditModal = function () {
        bookTableEditor = $('#editDialog').load("ApplicationScripts/editBook.html").dialog({
            autoOpen: false,
            modal: true,
            classes: {
                "ui-dialog": "modal-content",
                "ui-dialog-titlebar": "modal-header",
                "ui-dialog-title": "modal-title",
                "ui-dialog-titlebar-close": "close",
                "ui-dialog-content": "modal-body",
                "ui-dialog-buttonpane": "modal-footer"
            },
            buttons: [
                {
                    text: "Save book",
                    addClass: "btn btn-success",
                    click: updateBook()
                },
                {
                    text: "Close",
                    addClass: "btn btn-danger",
                    click: function () { $(this).dialog("close"); }
                }
            ]
        });
        $('#newBookBtn').click(function (e) {
            e.preventDefault();
            bookTableEditor.dialog("open");
        });
    };

    function updateBook() {
    }

    function format(rowData) {
        return '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
            '<tr>' +
            '<td>Book Id:</td>' +
            '<td>' + rowData.Id + '</td>' +
            '</tr>' +
            '<tr>' +
            '<td>Title:</td>' +
            '<td>' + rowData.Title + '</td>' +
            '</tr>' +
            '<tr>' +
            '<td>Year of Publishing:</td>' +
            '<td>' + rowData.PublishingYear + '</td>' +
            '</tr>' +
            '<tr>' +
            '<td>Amount of Pages:</td>' +
            '<td>' + rowData.PagesAmount + '</td>' +
            '</tr>' +
            '<tr>' +
            '<td>Rate:</td>' +
            '<td>' + rowData.Rate + '</td>' +
            '</tr>' +
            '<tr>' +
            '<td>Authors:</td>' +
            '<td>' + rowData.Authors + '</td>' +
            '</tr>' +
            '<tr>' +
            '</table>';
    };
}).apply(bookCatalog);