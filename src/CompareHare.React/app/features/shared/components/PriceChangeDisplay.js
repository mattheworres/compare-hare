import React from 'react';
import {Typography, withStyles} from '@material-ui/core';
import PropTypes from 'prop-types';
import {printMoney} from '../../shared/services/displayHelpers';
import {ArrowDropUp, ArrowDropDown} from '@material-ui/icons';

const styles = () => ({
  positive: {
    color: '#4caf50',
    verticalAlign: 'bottom'
  },
  negative: {
    color: '#f44336',
    verticalAlign: 'bottom'
  },
});

class PriceChangeDisplay extends React.PureComponent {
  render() {
    const {
      percentChange,
      amountChange,
      classes,
      variant,
      className
    } = this.props;

    const UP_ICON = <ArrowDropUp className={classes.negative} />
    const DOWN_ICON = <ArrowDropDown className={classes.positive} />

    const emptyDisplay = <>&mdash;</>;

    const percentChangeDisplay = percentChange
        ? ` (${(percentChange * 100).toFixed(2)}%)`
      : null;
    const amountChangePositive = amountChange && amountChange > 0;
    const colorClass = amountChangePositive ? classes.negative : classes.positive;

    return amountChange
        ? <Typography variant={variant} className={`${colorClass} ${className}`}>
            {amountChangePositive ? UP_ICON : DOWN_ICON}
            {printMoney(amountChange)}
            {percentChangeDisplay}
          </Typography>
        : <span className={className}>{emptyDisplay}</span>;
  }
}

PriceChangeDisplay.propTypes = {
  percentChange: PropTypes.number,
  amountChange: PropTypes.number,
  variant: PropTypes.string.isRequired,
  classes: PropTypes.object.isRequired,
  className: PropTypes.string
};

PriceChangeDisplay.defaultProps = {
  variant: 'subheading'
}

export default withStyles(styles)(PriceChangeDisplay);
