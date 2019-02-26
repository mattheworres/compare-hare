import React from 'react';
import {Map} from 'immutable';
import {Button, Form, Segment} from 'semantic-ui-react';
import {TextInput, PasswordInput} from '../../forms/components';
import PropTypes from 'prop-types';
import {LoginModel} from '../models';

export default function LoginForm({
  model,
  onFieldChange,
  onSubmit,
  disabled,
  validationErrors,
}) {
  return (
    <Form size="large" onSubmit={onSubmit} loading={disabled}>
      <Segment stacked>
        <TextInput
          id="email"
          fluid
          icon="user"
          iconPosition="left"
          autoComplete="username"
          placeholder="Email"
          maxLength={256}
          value={model.email}
          autoFocus
          formValidationErrors={validationErrors}
          onChange={onFieldChange}
        />
        <PasswordInput
          id="password"
          fluid
          icon="lock"
          placeholder="Password"
          iconPosition="left"
          autoComplete="current-password"
          value={model.password}
          formValidationErrors={validationErrors}
          onChange={onFieldChange}
          type="password"
        />
        <Button
          fluid
          className="primary"
          size="large"
          type="submit"
          disabled={disabled}
        >
          Login
        </Button>
      </Segment>
    </Form>
  );
}

LoginForm.propTypes = {
  model: PropTypes.instanceOf(LoginModel).isRequired,
  disabled: PropTypes.bool,
  validationErrors: PropTypes.instanceOf(Map).isRequired,
  onFieldChange: PropTypes.func.isRequired,
  onSubmit: PropTypes.func.isRequired,
};
