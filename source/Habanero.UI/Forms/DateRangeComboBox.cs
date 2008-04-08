//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Habanero.Util;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Provides a selection of common date range options which a user
    /// can select from, modifying a start date and end date depending
    /// on the selection.  The options to choose from, such as
    /// "Yesterday", "Last Week" or "This Year", can be modified by
    /// the developer.  When adding this control, add an event handler
    /// on the SelectionChangeCommitted event, and obtain the StartDate and
    /// EndDate properties at this time.
    /// <br/>
    /// NOTE: The start and end dates are the two boundaries of a
    /// potential range.  Whether they should be inclusive or exclusive
    /// depends on the developer and how they are implemented.  For
    /// instance, if today is Nov 13, Yesterday would provide a start
    /// date of Nov 12, 12am and an end date of Nov 13, 12am.
    /// <br/>
    /// To add additional menu options that aren't available, simply add the
    /// new text item to the ComboBox, using Items.Add.  When you pick up the
    /// SelectionChangeCommitted event, check if the new text string is selected
    /// in the Text property and use your own calculations instead of 
    /// fetching the StartDate and EndDate.
    /// </summary>
    public class DateRangeComboBox : ComboBox
    {
        /// <summary>
        /// Provides options that can be added to or removed from the
        /// ComboBox
        /// </summary>
        public enum DateOptions
        {
            /// <summary>
            /// The period from the start of the hour until now
            /// </summary>
            ThisHour,
            /// <summary>
            /// The period covering the previous hour before the start of the
            /// current one
            /// </summary>
            PreviousHour,
            /// <summary>
            /// The period covering the previous 60 minutes before this one
            /// </summary>
            Current60Minutes,
            /// <summary>
            /// The period from the start of the day until now
            /// </summary>
            Today,
            /// <summary>
            /// The period from the start of the previous day till
            /// the end of the previous day
            /// </summary>
            Yesterday,
            /// <summary>
            /// The period covering the last 24 hours up till now
            /// </summary>
            Current24Hours,
            /// <summary>
            /// The period from the first day of the week until now
            /// </summary>
            ThisWeek,
            /// <summary>
            /// The period from the first day of the previous week
            /// until the last day of the previous week
            /// </summary>
            PreviousWeek,
            /// <summary>
            /// The previous seven days, excluding today
            /// </summary>
            Previous7Days,
            /// <summary>
            /// The period from the start of the month until now
            /// </summary>
            ThisMonth,
            /// <summary>
            /// The period covering the previous month
            /// </summary>
            PreviousMonth,
            /// <summary>
            /// The period covering the previous 30 days, excluding today
            /// </summary>
            Previous30Days,
            /// <summary>
            /// The period covering the previous 31 days, excluding today
            /// </summary>
            Previous31Days,
            /// <summary>
            /// The period covering the start of the year until now
            /// </summary>
            ThisYear,
            /// <summary>
            /// The period covering the previous year
            /// </summary>
            PreviousYear,
            /// <summary>
            /// The period covering the last 365 days, excluding today
            /// </summary>
            Previous365Days,
            /// <summary>
            /// The period covering the 2 years up till now
            /// </summary>
            Current2Years,
            /// <summary>
            /// The period covering the 3 years up till now
            /// </summary>
            Current3Years,
            /// <summary>
            /// The period covering the 5 years up till now
            /// </summary>
            Current5Years,
            /// <summary>
            /// The period covering the 2 years before the current one
            /// </summary>
            Previous2Years,
            /// <summary>
            /// The period covering the 3 years before the current one
            /// </summary>
            Previous3Years,
            /// <summary>
            /// The period covering the 5 years before the current one
            /// </summary>
            Previous5Years
        }
        
        private Dictionary<DateOptions, string> _dateRangePairs;
        private List<DateOptions> _optionsToDisplay;

        private DateTime _startDate;
        private DateTime _endDate;

        private bool _ignoreTime;
        private TimeSpan _midnightOffset;
        private int _weekStartOffset;
        private int _monthStartOffset;
        private int _yearStartOffset;

        private Boolean _useFixedNowDate;
        private DateTime _fixedNow;

        /// <summary>
        /// Constructor to initialise a new ComboBox with a selection of
        /// date range options that are suited to a timeless system
        /// </summary>
        public DateRangeComboBox()
        {
            _optionsToDisplay = new List<DateOptions>();
            _optionsToDisplay.Add(DateOptions.Today);
            _optionsToDisplay.Add(DateOptions.Yesterday);
            _optionsToDisplay.Add(DateOptions.ThisWeek);
            _optionsToDisplay.Add(DateOptions.PreviousWeek);
            _optionsToDisplay.Add(DateOptions.Previous7Days);
            _optionsToDisplay.Add(DateOptions.ThisMonth);
            _optionsToDisplay.Add(DateOptions.PreviousMonth);
            _optionsToDisplay.Add(DateOptions.Previous31Days);
            _optionsToDisplay.Add(DateOptions.ThisYear);
            _optionsToDisplay.Add(DateOptions.PreviousYear);
            _optionsToDisplay.Add(DateOptions.Previous365Days);

            InitialiseValues();
        }

        /// <summary>
        /// Constructor that provides a specific list of options to display
        /// </summary>
        /// <param name="optionsToDisplay">A list of date range options</param>
        public DateRangeComboBox(List<DateOptions> optionsToDisplay)
        {
            _optionsToDisplay = optionsToDisplay;
            InitialiseValues();
        }

        /// <summary>
        /// Initialises the ComboBox with a list of values to display
        /// </summary>
        private void InitialiseValues()
        {
            SetDateRangePairs();
            BuildComboBoxList();

            _ignoreTime = false;
            _useFixedNowDate = false;
            _fixedNow = DateTime.Now;
            _weekStartOffset = 0;
            _monthStartOffset = 0;
            _yearStartOffset = 0;
        }

        /// <summary>
        /// Populates the ComboBox with the current set of date options
        /// </summary>
        private void BuildComboBoxList()
        {
            Items.Clear();
            Items.Add("(Date Ranges)");
            Text = Items[0].ToString();
            foreach (DateOptions options in _optionsToDisplay)
            {
                Items.Add(_dateRangePairs[options]);
            }
        }

        /// <summary>
        /// Populates the ComboBox with all available DateOptions, since
        /// the default constructor only provides a standardised collection
        /// </summary>
        public void UseAllDateOptions()
        {
            _optionsToDisplay = new List<DateOptions>();
            foreach (int constant in Enum.GetValues(typeof(DateOptions)))
            {
                AddDateOption((DateOptions)Enum.ToObject(typeof(DateOptions), constant));
            }
        }

        /// <summary>
        /// Sets the item in the ComboBox that first appears to the user
        /// </summary>
        /// <param name="displayString">The string to display</param>
        public void SetTopComboBoxItem(string displayString)
        {
            string oldText = Items[0].ToString();
            Items[0] = displayString;
            if (Text == oldText)
            {
                Text = Items[0].ToString();
            }
        }

        /// <summary>
        /// Creates a dictionary of enum and string pairs to define
        /// how each option is displayed and to recognise the appropriate
        /// option from the user's selection
        /// </summary>
        private void SetDateRangePairs()
        {
            _dateRangePairs = new Dictionary<DateOptions, string>();
 	        foreach (int constant in Enum.GetValues(typeof(DateOptions)))
 	        {
 	            string displayString = StringUtilities.DelimitPascalCase(Enum.GetName(typeof (DateOptions), constant), " ");
 	            _dateRangePairs.Add((DateOptions)Enum.ToObject(typeof(DateOptions), constant), displayString);
 	        }
        }

        /// <summary>
        /// Amends the display string for a given date option
        /// </summary>
        /// <param name="option">The date option to amend</param>
        /// <param name="newDisplayString">The display string to apply</param>
        public void SetDateRangeString(DateOptions option, string newDisplayString)
        {
            if (_dateRangePairs.ContainsKey(option))
            {
                if (_dateRangePairs.ContainsValue(newDisplayString)
                    && _dateRangePairs[option] != newDisplayString)
                {
                    throw new ArgumentException("A date range display string " +
                        "is being assigned, but that display string has already " +
                        "been used.");
                }
                int comboBoxPos = Items.IndexOf(_dateRangePairs[option]);
                Items[comboBoxPos] = newDisplayString;
                _dateRangePairs[option] = newDisplayString;
            }
            else
            {
                throw new ArgumentException("A date range string is being changed, " +
                    "but the given option does not exist in the collection " +
                    "of date options.");
            }
        }

        /// <summary>
        /// Returns the display string for the date range option supplied
        /// </summary>
        /// <param name="option">The date range enumeration</param>
        /// <returns>Returns the string if found, otherwise throws an
        /// ArgumentException</returns>
        public string GetDateRangeString(DateOptions option)
        {
            if (_dateRangePairs.ContainsKey(option))
            {
                return _dateRangePairs[option];
            }
            else
            {
                throw new ArgumentException("A date range option string is being " +
                    "accessed, but the given date range option does not exist.");
            }
        }

        /// <summary>
        /// Gets and sets the list of options to display.  If you intend
        /// to edit individual items in the list, either set the entire
        /// list once you have edited it, or use the Add and Remove methods
        /// provided by this class, otherwise the ComboBox list will not
        /// be refreshed.
        /// </summary>
        public List<DateOptions> OptionsToDisplay
        {
            get { return _optionsToDisplay; }
            set
            {
                _optionsToDisplay = value;
                BuildComboBoxList();
            }
        }

        /// <summary>
        /// Adds a date range option to the current list of options available
        /// </summary>
        /// <param name="option">The date range option to add</param>
        public void AddDateOption(DateOptions option)
        {
            if (!_optionsToDisplay.Contains(option))
            {
                _optionsToDisplay.Add(option);
                Items.Add(_dateRangePairs[option]);
            }
        }

        /// <summary>
        /// Removes a date range option from the current list of options available
        /// </summary>
        /// <param name="option">The date range option to remove</param>
        public void RemoveDateOption(DateOptions option)
        {
            if (_optionsToDisplay.Contains(option))
            {
                _optionsToDisplay.Remove(option);
                Items.Remove(_dateRangePairs[option]);
            }
        }

        /// <summary>
        /// Sets the current date (eg. DateTime.Now or FixedNowDate) in all calculations to 12am.
        /// Use caution when using this together with a MidnightOffset, in which
        /// case you may rather want to manually edit the time just before calling
        /// StartDate and EditDate (use UseFixedNowDate and FixedNowDate).
        /// </summary>
        public bool IgnoreTime
        {
            get { return _ignoreTime; }
            set { _ignoreTime = value; }
        }

        /// <summary>
        /// Gets and sets the amount of time to add or subtract from
        /// midnight when calculating date ranges.  This option will
        /// typically be used where a shift operates on a different
        /// pattern to 12am to 12am (the default).  If, for instance,
        /// an industry's operational day runs from 6am to 6am, this
        /// property can be set with a TimeSpan that adds 6 hours.
        /// Conversely, if the day starts 2 hours earlier, at 10pm the
        /// previous evening, set the property with a TimeSpan that
        /// subtracts 2 hours.
        /// </summary>
        public TimeSpan MidnightOffset
        {
            get { return _midnightOffset; }
            set
            {
                if (Math.Abs(value.TotalHours) >= 24)
                {
                    throw new ArgumentException("A midnight offset must be " +
                        "less than 24 hours.");
                }
                _midnightOffset = value;
            }
        }

        /// <summary>
        /// Gets and sets the number of days to add or subtract from
        /// Monday to redefine the first day of the week.  If Sunday
        /// is the first day of the week for the given application,
        /// then this property can be set with -1.  If Tuesday is the
        /// first day then use 1 (1+1=2).
        /// </summary>
        public int WeekStartOffset
        {
            get { return _weekStartOffset; }
            set { _weekStartOffset = value; }
        }

        /// <summary>
        /// Gets and sets the number of days to add or subtract from
        /// the first day of the month in order to adjust which day
        /// is typically the first of the month.  If the 5th is the typical start
        /// of a new month for the given application,
        /// then this property can be set to 4 (1+4=5).
        /// </summary>
        public int MonthStartOffset
        {
            get { return _monthStartOffset; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Month start offset cannot be negative.");
                }
                _monthStartOffset = value;
            }
        }

        /// <summary>
        /// Gets and sets the number of months to add or subtract from
        /// January to redefine the first month of the year.  For example,
        /// if March is the first month of the new year for the given application,
        /// then this property can be set with 2 (1+2=3).
        /// </summary>
        public int YearStartOffset
        {
            get { return _yearStartOffset; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Year start offset cannot be negative.");
                }
                _yearStartOffset = value;
            }
        }

        /// <summary>
        /// Gets and sets whether the date used to calculate date ranges
        /// should be DateTime.Now or a fixed date that is specified.
        /// When false, all date ranges are calculated based on DateTime.Now.
        /// Setting this property to true allows you to use an alternative
        /// fixed date as your "Now" value, using the FixedNow property.
        /// </summary>
        public bool UseFixedNowDate
        {
            get { return _useFixedNowDate; }
            set { _useFixedNowDate = value; }
        }

        /// <summary>
        /// Gets and sets a fixed date used to calculate date ranges, rather
        /// than DateTime.Now.  The UseFixedNowDate property must be set to
        /// true, otherwise this property will be ignored.
        /// </summary>
        public DateTime FixedNowDate
        {
            get { return _fixedNow; }
            set { _fixedNow = value; }
        }

        /// <summary>
        /// Returns the appropriate date used for date range calculations,
        /// depending on whether UseFixedNowDate has been set.
        /// </summary>
        private DateTime Now
        {
            get
            {
                DateTime now = DateTime.Now;
                if (UseFixedNowDate) now = FixedNowDate;
                
                if (IgnoreTime)
                {
                    now = new DateTime(now.Year, now.Month, now.Day);
                }
                return now;
            }
        }

        /// <summary>
        /// Returns the start date for the currently selected date range option,
        /// or DateTime.MinValue if no valid option is selected
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                CalculateDates();
                return _startDate;
            }
        }

        /// <summary>
        /// Returns the end date for the currently selected date range option,
        /// or DateTime.MaxValue if no valid option is selected
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                CalculateDates();
                return _endDate;
            }
        }

        /// <summary>
        /// Calculates the start and end dates based on the currently
        /// selected ComboBox item
        /// </summary>
        private void CalculateDates()
        {
            if (!_dateRangePairs.ContainsValue(Text))
            {
                _startDate = DateTime.MinValue;
                _endDate = DateTime.MaxValue;
                return;
            }

            DateOptions option = DateOptions.Today;
            bool found = false;
            foreach (KeyValuePair<DateOptions, string> pair in _dateRangePairs)
            {
                if (pair.Value == (string)SelectedItem)
                {
                    option = pair.Key;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                _startDate = DateTime.MinValue;
                _endDate = DateTime.MaxValue;
                return;
            }

            switch (option)
            {
                case DateOptions.ThisHour:
                    {
                        _startDate = HourStart(Now);
                        _endDate = Now;
                        break;
                    }
                case DateOptions.PreviousHour:
                    {
                        _startDate = HourStart(Now).AddHours(-1);
                        _endDate = HourStart(Now);
                        break;
                    }
                case DateOptions.Current60Minutes:
                    {
                        _startDate = Now.AddHours(-1);
                        _endDate = Now;
                        break;
                    }
                case DateOptions.Today:
                    {
                        _startDate = DayStart(Now);
                        _endDate = Now;
                        break;
                    }
                case DateOptions.Yesterday:
                    {
                        _startDate = DayStart(Now.AddDays(-1));
                        _endDate = DayStart(Now);
                        break;
                    }
                case DateOptions.Current24Hours:
                    {
                        _startDate = Now.AddDays(-1);
                        _endDate = Now;
                        break;
                    }
                case DateOptions.ThisWeek:
                    {
                        _startDate = WeekStart(Now);
                        _endDate = Now;
                        break;
                    }
                case DateOptions.PreviousWeek:
                    {
                        _startDate = WeekStart(Now).AddDays(-7);
                        _endDate = WeekStart(Now);
                        break;
                    }
                case DateOptions.Previous7Days:
                    {
                        _startDate = DayStart(Now).AddDays(-7);
                        _endDate = DayStart(Now);
                        break;
                    }
                case DateOptions.ThisMonth:
                    {
                        _startDate = MonthStart(Now);
                        _endDate = Now;
                        break;
                    }
                case DateOptions.PreviousMonth:
                    {
                        _startDate = MonthStart(Now).AddMonths(-1);
                        _endDate = MonthStart(Now);
                        break;
                    }
                case DateOptions.Previous30Days:
                    {
                        _startDate = DayStart(Now).AddDays(-30);
                        _endDate = DayStart(Now);
                        break;
                    }
                case DateOptions.Previous31Days:
                    {
                        _startDate = DayStart(Now).AddDays(-31);
                        _endDate = DayStart(Now);
                        break;
                    }
                case DateOptions.ThisYear:
                    {
                        _startDate = YearStart(Now);
                        _endDate = Now;
                        break;
                    }
                case DateOptions.PreviousYear:
                    {
                        _startDate = YearStart(Now).AddYears(-1);
                        _endDate = YearStart(Now);
                        break;
                    }
                case DateOptions.Previous365Days:
                    {
                        _startDate = DayStart(Now).AddDays(-365);
                        _endDate = DayStart(Now);
                        break;
                    }
                case DateOptions.Current2Years:
                    {
                        _startDate = Now.AddYears(-2);
                        _endDate = Now;
                        break;
                    }
                case DateOptions.Current3Years:
                    {
                        _startDate = Now.AddYears(-3);
                        _endDate = Now;
                        break;
                    }
                case DateOptions.Current5Years:
                    {
                        _startDate = Now.AddYears(-5);
                        _endDate = Now;
                        break;
                    }
                case DateOptions.Previous2Years:
                    {
                        _startDate = YearStart(Now).AddYears(-2);
                        _endDate = YearStart(Now);
                        break;
                    }
                case DateOptions.Previous3Years:
                    {
                        _startDate = YearStart(Now).AddYears(-3);
                        _endDate = YearStart(Now);
                        break;
                    }
                case DateOptions.Previous5Years:
                    {
                        _startDate = YearStart(Now).AddYears(-5);
                        _endDate = YearStart(Now);
                        break;
                    }
                default:
                    {
                        _startDate = DateTime.MinValue;
                        _endDate = DateTime.MaxValue;
                        break;
                    }
            }
        }

        /// <summary>
        /// Converts the given date to the start of the hour
        /// </summary>
        /// <param name="date">The date to convert</param>
        /// <returns>Returns the converted date</returns>
        private static DateTime HourStart(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0, 0);
        }

        /// <summary>
        /// Converts the given date to the previous midnight,
        /// factoring in the midnight offset
        /// </summary>
        /// <param name="date">The date to convert</param>
        /// <returns>Returns the converted date</returns>
        private DateTime DayStart(DateTime date)
        {
            DateTime midnight = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
            DateTime newMidnight = midnight.Add(MidnightOffset);
            if (newMidnight > date) return newMidnight.AddDays(-1);
            double dif = (date - newMidnight).TotalHours;
            if (dif > 24) return newMidnight.AddDays(1);
            else return newMidnight;
        }

        /// <summary>
        /// Converts the given date to the previous Monday midnight,
        /// factoring in the midnight offset and week start
        /// </summary>
        /// <param name="date">The date to convert</param>
        /// <returns>Returns the converted date</returns>
        private DateTime WeekStart(DateTime date)
        {
            DateTime monday = date;
            while (monday.DayOfWeek != DayOfWeek.Monday)
            {
                monday = monday.AddDays(-1);
            }
            DateTime startOfWeek = new DateTime(monday.Year, monday.Month, monday.Day);
            startOfWeek = startOfWeek.AddDays(WeekStartOffset).Add(MidnightOffset);
            if (startOfWeek > date) return startOfWeek.AddDays(-7);
            //double dif = (date - monday).TotalDays;
            //if (dif > 7) return monday.AddDays(7);
            if (WeekStartOffset < 0)
            {
                DateTime closer = new DateTime(monday.Year, monday.Month, monday.Day + 7);
                closer = closer.AddDays(WeekStartOffset).Add(MidnightOffset);
                if (closer < date) return closer;
            }
            return startOfWeek;
        }

        /// <summary>
        /// Converts the given date to the first day of the month,
        /// factoring in the month start and midnight offset
        /// </summary>
        /// <param name="date">The date to convert</param>
        /// <returns>Returns the converted date</returns>
        private DateTime MonthStart(DateTime date)
        {
            DateTime first = new DateTime(date.Year, date.Month, 1);
            first = first.AddDays(MonthStartOffset).Add(MidnightOffset);
            if (first > date)
            {
                first = first.AddMonths(-1);
            }
            if (MidnightOffset < new TimeSpan(0, 0, 0, 0, 0))
            {
                DateTime closer = new DateTime(date.Year, date.Month + 1, 1);
                closer = closer.AddDays(MonthStartOffset).Add(MidnightOffset);
                if (closer < date) return closer;
            }
            return first;
        }

        /// <summary>
        /// Converts the given date to the first day of the year,
        /// factoring in the year start, month start and midnight offset
        /// </summary>
        /// <param name="date">The date to convert</param>
        /// <returns>Returns the converted date</returns>
        private DateTime YearStart(DateTime date)
        {
            DateTime first = new DateTime(date.Year, 1, 1);
            first = first.AddMonths(YearStartOffset).AddDays(MonthStartOffset).Add(MidnightOffset);
            if (first > date)
            {
                first = first.AddYears(-1);
            }
            if (MidnightOffset < new TimeSpan(0, 0, 0, 0, 0))
            {
                DateTime closer = new DateTime(date.Year + 1, 1, 1);
                closer = closer.AddMonths(YearStartOffset).AddDays(MonthStartOffset).Add(MidnightOffset);
                if (closer < date) return closer;
            }
            return first;
        }
    }
}