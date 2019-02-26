import toastr from 'toastr';

export default function(error, history, errorMessage) {
  const {status} = error.response || {};

  if (!status) throw error;

  const BAD_REQUEST = 400;
  const UNAUTHORIZED = 401;
  const FORBIDDEN = 403;
  const PATH_FOR_UNAUTHORIZED = '/login';
  const PATH_FOR_FORBIDDEN = '/dashboard';
  const isValidationError = Boolean(
    status === BAD_REQUEST && error && error.response && error.response.data,
  );

  if (status === FORBIDDEN) {
    history.push(PATH_FOR_FORBIDDEN);
    toastr.error(
      'You have been redirected to your dashboard. Your request required access privileges not assigned to your account.',
      'Forbidden',
    );
  } else if (status === UNAUTHORIZED) {
    history.push(PATH_FOR_UNAUTHORIZED);
    toastr.info('Please log in and try again.', 'Unauthorized');
  } else if (!isValidationError) {
    toastr.error(
      errorMessage ||
        'The server encountered an error. Please try again later.',
      'Error',
    );
  }
}
