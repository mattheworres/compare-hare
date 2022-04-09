import * as Yup from 'yup';

export const addProduct1stFormValidationSchema = Yup.object({
  name: Yup.string()
    .required('Product name is required')
    .min(4, 'Name must be at least 4 characters long')
    .max(512, 'Name cannot be longer than 512 characters'),
});

export const addProduct1stFormDefaultValues = {
  name: '',
};

export const addProduct2ndFormValidationSchema = Yup.object({
  productRetailer: Yup.number()
    .integer('Product retailer must be a whole number (why are you seeing this??)')
    .required('You must pick a retailer from the list first'),
  otherRetailerDisplayName: Yup.string()
    .when(['productRetailer'], {
      is: 1001,
      // is: productRetailer => {
      //   console.log('Loggita!', productRetailer);
      //   return parseInt(productRetailer, 10) === 1001
      // }, // TODO: magic number
      then: Yup.string()
        .required('You must give this retailer a custom display name')
        .min(4, 'Other retailer needs a name with at least 4 characters')
        .max(512, 'Other retailer can\'t have a name longer than 512 characters')
        // .test('should-be-filled-out', 'You must give this retailer a custom name', function(value) {
        //   console.log('Hey, bro, whatdup', value);
        //   return value.length < 4;
        // }),
    })
});

export const addProduct2ndFormDefaultValues = {
  productRetailer: null,
  otherRetailerDisplayName: ''
};
