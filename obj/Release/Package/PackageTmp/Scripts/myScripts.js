
   

    $(document).ready(function () {

        

        $.validator.methods.range = function (value, element, param) {
            var globalizedValue = value.replace(",", ".");
            return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
        }

        

        $.validator.methods.number = function (value, element) {
            return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
        }

        $(function () {
            $.validator.methods.date = function (value, element) {
                if ($.browser.webkit) {

                    //ES - Chrome does not use the locale when new Date objects instantiated:
                    var d = new Date();

                    return this.optional(element) || !/Invalid|NaN/.test(new Date(d.toLocaleDateString(value)));
                }
                else {

                    return this.optional(element) || !/Invalid|NaN/.test(new Date(value));
                }
            };
        });

       
        
            $(".print").click(function () { window.print() });

       
            

            if (navigator.userAgent.indexOf('Chrome') != -1) {
                $('input[type=date]').on('click', function (event) {
                    event.preventDefault();
                });
            }

    });

    
   

   
    
    


