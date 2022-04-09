import * as Yup from 'yup';

export const addProduct1stFormValidationSchema = Yup.object({
  name: Yup.string()
    .required('Product name is required')
    .max(512, 'Name cannot be longer than 512 characters')
    .min(8, 'Name must be at least 8 characters long')
});

export const addProduct1stFormDefaultValues = {
  name: '',
};
