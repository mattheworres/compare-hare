import React from 'react';
import PropTypes from 'prop-types';
import autobind from 'class-autobind';
import {
  Stepper,
  Step,
  StepLabel
} from '@material-ui/core';

class PaPowerSteps extends React.PureComponent {
  constructor(props) {
    super(props);

    autobind(this);
  }

  render() {
    const { stepperClass, activeStepIndex } = this.props;

    if (activeStepIndex === null) {
      return null;
    }

    const isDistributor = activeStepIndex === 0;
    const distributorCompleted = activeStepIndex > 0;
    const isName = activeStepIndex === 1;
    const nameCompleted = activeStepIndex > 1;
    const isFlags = activeStepIndex === 2;
    const flagsCompleted = activeStepIndex > 2;
    const isReview = activeStepIndex === 3;

    return <Stepper className={stepperClass}>
          <Step active={isDistributor} completed={distributorCompleted}>
            <StepLabel>Distributor</StepLabel>
          </Step>
          <Step active={isName} completed={nameCompleted}>
            <StepLabel>Name</StepLabel>
          </Step>
          <Step active={isFlags} completed={flagsCompleted}>
            <StepLabel>Flags</StepLabel>
          </Step>
          <Step active={isReview}>
            <StepLabel>Review</StepLabel>
          </Step>
        </Stepper>;
  }
};

PaPowerSteps.propTypes = {
  stepperClass: PropTypes.string.isRequired,
  activeStepIndex: PropTypes.number
}

export default PaPowerSteps;
