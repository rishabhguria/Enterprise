<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <!--Third Friday check-->
  <msxsl:script language="C#" implements-prefix="my">
    public string Now(int year, int month)
    {
    DateTime thirdFriday= new DateTime(year, month, 15);
    while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
    {
    thirdFriday = thirdFriday.AddDays(1);
    }
    return thirdFriday.ToString();
    }
  </msxsl:script>

	<msxsl:script language="C#" implements-prefix="my">

		public static string NowSwap(int year, int month, int date)
		{
		DateTime weekEnd= new DateTime(year, month, date);
		weekEnd = weekEnd.AddDays(1);
		while (weekEnd.DayOfWeek == DayOfWeek.Saturday || weekEnd.DayOfWeek == DayOfWeek.Sunday)
		{
		weekEnd = weekEnd.AddDays(1);
		}
		return weekEnd.ToString();
		}

	</msxsl:script>


  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month='JAN'">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month='FEB'">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month='MAR'">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month='APR'">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month='MAY'">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month='JUN'">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month='JUL'">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month='AUG'">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month='SEP'">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month='OCT'">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month='NOV'">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month='DEC'">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month='JAN'">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month='FEB'">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month='MAR'">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month='APR'">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month='MAY'">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month='JUN'">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month='JUL'">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month='AUG'">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month='SEP'">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month='OCT'">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month='NOV'">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month='DEC'">
          <xsl:value-of select="'X'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:param name="Suffix"/>
    <xsl:if test="substring(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),' '),1,1)='C' or substring(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),' '),1,1)='P'">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before(normalize-space(COL3),' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL3),' '),' '),' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring-before(substring-after(normalize-space(COL3),' '),' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),3,2)"/>
      </xsl:variable>
      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),' '),1,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),' '),'##.00')"/>
      </xsl:variable>
      <xsl:variable name="MonthCodVar">
        <xsl:call-template name="MonthCode">
          <xsl:with-param name="Month" select="$ExpiryMonth"/>
          <xsl:with-param name="PutOrCall" select="$PutORCall"/>
        </xsl:call-template>
      </xsl:variable>
      <xsl:variable name="Day">
        <xsl:choose>
          <xsl:when test="substring($ExpiryDay,1,1)='0'">
            <xsl:value-of select="substring($ExpiryDay,2,1)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$ExpiryDay"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <xsl:variable name="ThirdFriday">
        <xsl:choose>
          <xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
            <xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
          </xsl:when>
        </xsl:choose>
      </xsl:variable>
      <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>
    </xsl:if>
  </xsl:template>



  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>



  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL10"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Position)">
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'MS'"/>
            </xsl:variable>

            <xsl:variable name = "PB_COMPANY" >
              <xsl:value-of select="COL25"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
            </xsl:variable>


            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:value-of select="COL9"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <AccountName>
              <xsl:choose>
                <xsl:when test="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

           
            <xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL38)"/>
            </xsl:variable>
            <Symbol>
              <xsl:choose>
                <xsl:when test ="$PRANA_SYMBOL != ''">
                  <xsl:value-of select ="$PRANA_SYMBOL"/>
                </xsl:when>

                
                <xsl:when test="$varSymbol!=''">
                  <xsl:value-of select="$varSymbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_COMPANY"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>

            <xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="normalize-space(COL7)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_BROKER_ID">
              <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
            </xsl:variable>

            <CounterPartyID>
              <xsl:choose>
                <xsl:when test="number($PRANA_BROKER_ID)">
                  <xsl:value-of select="$PRANA_BROKER_ID"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CounterPartyID>

			  <xsl:variable name="PB_CURRENCY_NAME">
				  <xsl:value-of select="COL27"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_CURRENCY_ID">
				  <xsl:value-of select="document('../ReconMappingXml/CurrencyMapping.xml')/CurrencyMapping/PB[@Name=$PB_NAME]/CurrencyData[@CurrencyName=$PB_CURRENCY_NAME]/@PranaCurrencyCode"/>
			  </xsl:variable>

			  <CurrencyID>
				  <xsl:choose>
					  <xsl:when test="number($PRANA_CURRENCY_ID)">
						  <xsl:value-of select="$PRANA_CURRENCY_ID"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </CurrencyID>


            <PBSymbol>
              <xsl:value-of select="$PB_COMPANY"/>
            </PBSymbol>


            <NetPosition>
              <xsl:choose>
                <xsl:when  test="$Position &gt; 0">
                  <xsl:value-of select="$Position"/>
                </xsl:when>
                <xsl:when test="$Position &lt; 0">
                  <xsl:value-of select="$Position * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>
            
            <xsl:variable name="varCostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL11"/>
              </xsl:call-template>
            </xsl:variable>
            <CostBasis>
              <xsl:choose>
                <xsl:when  test="$varCostBasis &gt; 0">
                  <xsl:value-of select="$varCostBasis"/>
                </xsl:when>
                <xsl:when test="$varCostBasis &lt; 0">
                  <xsl:value-of select="$varCostBasis * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </CostBasis>

            <xsl:variable name="varCommission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL14"/>
              </xsl:call-template>
            </xsl:variable>
            <Commission>
              <xsl:choose>
                <xsl:when  test="$varCommission &gt; 0">
                  <xsl:value-of select="$varCommission"/>
                </xsl:when>
                <xsl:when test="$varCommission &lt; 0">
                  <xsl:value-of select="$varCommission * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>



            <xsl:variable name="varOtherBrokerFees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL13"/>
              </xsl:call-template>
            </xsl:variable>
            <Fees>
              <xsl:choose>
                <xsl:when test="$varOtherBrokerFees &gt; 0">
                  <xsl:value-of select="$varOtherBrokerFees"/>
                </xsl:when>
                <xsl:when test="$varOtherBrokerFees &lt; 0">
                  <xsl:value-of select="$varOtherBrokerFees * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Fees>



            <xsl:variable name="varDateTime">
              <xsl:value-of select="COL5"/>
            </xsl:variable>
            <PositionStartDate>
              <xsl:value-of select="$varDateTime"/>
            </PositionStartDate>



            <xsl:variable name="varSide">
              <xsl:value-of select="COL3"/>
            </xsl:variable>
            <SideTagValue>

                  <xsl:choose>
                    <xsl:when  test="$varSide='Buy'">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>

                    <xsl:when  test="$varSide='Sell'">
                      <xsl:value-of select="'2'"/>
                    </xsl:when>

                    <xsl:when  test="$varSide='Short'">
                      <xsl:value-of select="'5'"/>
                    </xsl:when>
                    <xsl:when  test="$varSide='Cover'">
                      <xsl:value-of select="'B'"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                
            </SideTagValue>

			  <xsl:variable name="SecFee">
				  <xsl:choose>

					  <xsl:when test="COL3='Sell' or COL3='Short'">
						  <xsl:value-of select="(COL10 * COL11) * (0.0000221 )"/>
					  </xsl:when>


					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <SecFee>

				  <xsl:value-of select="$SecFee"/>

			  </SecFee>


			  <xsl:variable name ="varDate" select="'12/31/2020'"/>
            <!--<PositionStartDate>
              <xsl:value-of select="'12/31/2020'"/>
            </PositionStartDate>-->

            <xsl:variable name="varOrigTransDate">
              <xsl:value-of select="$varDate"/>
            </xsl:variable>
			
			
			   <xsl:if test="number(COL10) and COL26='EQUITY SWAP'">

              <IsSwapped>
                <xsl:value-of select ="1"/>
              </IsSwapped>

              <SwapDescription>
                <xsl:value-of select ="'SWAP'"/>
              </SwapDescription>

              <DayCount>
                <xsl:value-of select ="365"/>
              </DayCount>

              <ResetFrequency>
                <xsl:value-of select ="'Monthly'"/>
              </ResetFrequency>

              <OrigTransDate>
                <xsl:value-of select ="$varOrigTransDate"/>
              </OrigTransDate>

              <xsl:variable name="varPrevMonth">
                <xsl:choose>
                  <xsl:when test ="number(substring-before($varOrigTransDate,'/')) = 1">
                    <xsl:value-of select ="12"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="number(substring-before($varOrigTransDate,'/'))-1"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <xsl:variable name ="varSYear">
                <xsl:choose>
                  <xsl:when test ="number(substring-before($varOrigTransDate,'/')) = 1">
                    <xsl:value-of select ="number(substring-after(substring-after($varOrigTransDate,'/'),'/')) -1"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="number(substring-after(substring-after($varOrigTransDate,'/'),'/'))"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="varYear">
                <xsl:choose>
                  <xsl:when test="contains($varOrigTransDate,'/')">
                    <xsl:value-of select="substring-after(substring-after($varOrigTransDate,'/'),'/')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="substring-after(substring-after($varOrigTransDate,'-'),'-')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="Day">
                <xsl:choose>
                  <xsl:when test="contains($varOrigTransDate,'/')">
                    <xsl:choose>
                      <xsl:when test="string-length(number(substring-before(substring-after($varOrigTransDate,'/'),'/'))) = 1">
                        <xsl:value-of select="concat(0,substring-before(substring-after($varOrigTransDate,'/'),'/'))"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="substring-before(substring-after($varOrigTransDate,'/'),'/')"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="string-length(number(substring-before(substring-after($varOrigTransDate,'-'),'-'))) = 1">
                        <xsl:value-of select="concat(0,substring-before(substring-after($varOrigTransDate,'-'),'-'))"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="substring-before(substring-after($varOrigTransDate,'-'),'-')"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="Month">
                <xsl:choose>
                  <xsl:when test="contains($varOrigTransDate,'/')">
                    <xsl:choose>
                      <xsl:when test="string-length(number(substring-before($varOrigTransDate,'/'))) = 1">
                        <xsl:value-of select="concat(0,substring-before($varOrigTransDate,'/'))"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="substring-before($varOrigTransDate,'/')"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="string-length(number(substring-before($varOrigTransDate,'-'))) = 1">
                        <xsl:value-of select="concat(0,substring-before($varOrigTransDate,'-'))"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="substring-before($varOrigTransDate,'-')"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>


              <xsl:variable name="SettleDate">
                <xsl:value-of select='my:NowSwap(number($varYear),number($Month),number($Day))'/>
              </xsl:variable>

              <FirstResetDate>
                <xsl:value-of select ="$SettleDate"/>
              </FirstResetDate>
			  <!--<CounterpartyID>
                <xsl:value-of select ="$SettleDate"/>
              </CounterpartyID>-->
			  
            </xsl:if>



          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <!-- variable declaration for lower to upper case -->

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

  <!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>


