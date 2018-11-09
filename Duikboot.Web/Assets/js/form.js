//Form to register new members
new Vue({
    el: '#app',
    data: {
        firstName: '',
        surName: '',
        email: '',
        phoneNumber: ''
    },
    methods: {
        getFormValues(submitEvent) {    
            
            const user ={
                    "firstName": this.firstName,
                    "surName": this.surName,
                    "email": this.email,
                    "phoneNumber": this.phoneNumber
                }

            axios.post('/Api/Registration/Submit', user)
                .catch((error) => {
                    console.log(error);
                })
                .then((response) => {
                    console.log(response);
                });
                
        }
    }
});