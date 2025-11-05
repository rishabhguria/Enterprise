<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
  <xsl:output method="xml" indent="yes"/>

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

    <!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
  </xsl:template>

  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month=1 ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=2 ">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=3 ">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=4 ">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=5 ">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month=6 ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=7  ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=8  ">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=9 ">
          <xsl:value-of select="'I'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'J'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'K'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'L'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$PutOrCall='P'">
      <xsl:choose>
        <xsl:when test="$Month=1 ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=2 ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=3 ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=4 ">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=5 ">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=6 ">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=7  ">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=8  ">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=9 ">
          <xsl:value-of select="'U'"/>
        </xsl:when>
        <xsl:when test="$Month=10 ">
          <xsl:value-of select="'V'"/>
        </xsl:when>
        <xsl:when test="$Month=11 ">
          <xsl:value-of select="'W'"/>
        </xsl:when>
        <xsl:when test="$Month=12 ">
          <xsl:value-of select="'X'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <xsl:template name="Date">
    <xsl:param name="Month"/>
    <xsl:choose>
      <xsl:when test="$Month='Jan'">
        <xsl:value-of select="1"/>
      </xsl:when>
      <xsl:when test="$Month='Feb'">
        <xsl:value-of select="2"/>
      </xsl:when>
      <xsl:when test="$Month='Mar'">
        <xsl:value-of select="3"/>
      </xsl:when>
      <xsl:when test="$Month='Apr'">
        <xsl:value-of select="4"/>
      </xsl:when>
      <xsl:when test="$Month='May'">
        <xsl:value-of select="5"/>
      </xsl:when>
      <xsl:when test="$Month='Jun'">
        <xsl:value-of select="6"/>
      </xsl:when>
      <xsl:when test="$Month='Jul'">
        <xsl:value-of select="7"/>
      </xsl:when>
      <xsl:when test="$Month='Aug'">
        <xsl:value-of select="8"/>
      </xsl:when>
      <xsl:when test="$Month='Sep'">
        <xsl:value-of select="9"/>
      </xsl:when>
      <xsl:when test="$Month='Oct'">
        <xsl:value-of select="10"/>
      </xsl:when>
      <xsl:when test="$Month='Nov'">
        <xsl:value-of select="11"/>
      </xsl:when>
      <xsl:when test="$Month='Dec'">
        <xsl:value-of select="12"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:param name="Suffix"/>
    <xsl:if test="COL4='OPT'">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before($Symbol,' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring(substring-after($Symbol,' '),5,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring(substring-after($Symbol,' '),3,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-after($Symbol,' '),1,2)"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-after($Symbol,' '),7,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring(substring-after($Symbol,' '),8) div 1000,'#.00')"/>
      </xsl:variable>


      <xsl:variable name="MonthCodeVar">
        <xsl:call-template name="MonthCode">
          <xsl:with-param name="Month" select="number($ExpiryMonth)"/>
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
      <!--<xsl:choose>
				<xsl:when test="number($ExpiryMonth)=1 and $ExpiryYear='15'">

					<xsl:choose>
						<xsl:when test="substring(substring-after($ThirdFriday,'/'),1,2) = ($ExpiryDay - 1)">
							<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>

				<xsl:otherwise>-->
      <xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
      <!--</xsl:otherwise>-->
      <!--

			</xsl:choose>-->
    </xsl:if>
  </xsl:template>
  <xsl:template name="spaces">
    <xsl:param name="count"/>
    <xsl:if test="number($count)">
      <xsl:call-template name="spaces">
        <xsl:with-param name="count" select="$count - 1"/>
      </xsl:call-template>
    </xsl:if>
    <xsl:value-of select="' '"/>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="LastPrice">
          <xsl:value-of select="COL22"/>
        </xsl:variable>

        <xsl:if test="number($LastPrice)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'IB'"/>
            </xsl:variable>

            <xsl:variable name = "PB_SYMBOL_NAME" >
              <xsl:value-of select ="normalize-space(COL7)"/>
            </xsl:variable>

            <xsl:variable name="Currency" select="''"/>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SUFFIX_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$Currency]/@PranaSuffixCode"/>
            </xsl:variable>

			  <xsl:variable name="PB_ROOT_NAME">
				  <xsl:value-of select="substring(COL6,1,2)"/>
			  </xsl:variable>


			  <xsl:variable name ="PRANA_ROOT_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/Exchange[@Name=$PB_NAME]/SymbolData[@InstrmentCode = $PB_ROOT_NAME]/@UnderlyingCode"/>
			  </xsl:variable>
            <xsl:variable name ="FUTURE_SUFFIX_NAME">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@Currency=$Currency]/@ExchangeName"/>
            </xsl:variable>

            <xsl:variable  name="FUTURE_MULTIPLIER">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME]/@StrikeMul"/>
            </xsl:variable>

            <xsl:variable  name="FUTURE_FLAG">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME]/@ExpFlag"/>
            </xsl:variable>

            <xsl:variable name="Multiplier">
              <xsl:choose>
                <xsl:when test="number($FUTURE_MULTIPLIER)">
                  <xsl:value-of select="$FUTURE_MULTIPLIER"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="1"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varMonth">
              <xsl:choose>
                <xsl:when test="contains(COL6,' ')">
                  <xsl:value-of select="substring-before(substring-after(normalize-space(COL6),' '),' ')"/>
                  </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring(normalize-space(COL6),3,1)"/>
                </xsl:otherwise>
              </xsl:choose>
             
            </xsl:variable>

            <xsl:variable name="varMonthCode">
              <xsl:choose>
                <xsl:when test ="contains(COL6,' ')">
                  <xsl:choose>
                    <xsl:when test="$varMonth='JAN'">
                      <xsl:value-of select="'F'"/>
                    </xsl:when>
                    <xsl:when test="$varMonth='FEB'">
                      <xsl:value-of select="'G'"/>
                    </xsl:when>
                    <xsl:when test="$varMonth='MAR'">
                      <xsl:value-of select="'H'"/>
                    </xsl:when>
                    <xsl:when test="$varMonth='APR'">
                      <xsl:value-of select="'J'"/>
                    </xsl:when>
                    <xsl:when test="$varMonth='MAY'">
                      <xsl:value-of select="'K'"/>
                    </xsl:when>
                    <xsl:when test="$varMonth='JUN'">
                      <xsl:value-of select="'M'"/>
                    </xsl:when>
                    <xsl:when test="$varMonth='JUL'">
                      <xsl:value-of select="'N'"/>
                    </xsl:when>
                    <xsl:when test="$varMonth='AUG'">
                      <xsl:value-of select="'Q'"/>
                    </xsl:when>
                    <xsl:when test="$varMonth='SEP'">
                      <xsl:value-of select="'U'"/>
                    </xsl:when>
                    <xsl:when test="$varMonth='OCT'">
                      <xsl:value-of select="'V'"/>
                    </xsl:when>
                    <xsl:when test="$varMonth='NOV'">
                      <xsl:value-of select="'X'"/>
                    </xsl:when>
                    <xsl:when test="$varMonth='DEC'">
                      <xsl:value-of select="'Z'"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring(COL6,3,1)"/>
                </xsl:otherwise>
              </xsl:choose>
              
            </xsl:variable>

            <xsl:variable name="varYear">

              <xsl:choose>
                <xsl:when test="contains(COL6,' ')">
                  <xsl:value-of select="substring(substring-after(substring-after(normalize-space(COL6),' '),' '),2,1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring(normalize-space(COL6),4,1)"/>
                </xsl:otherwise>
              </xsl:choose>

           
            </xsl:variable>

            <xsl:variable name="MonthYearCode">
              <xsl:choose>
                <xsl:when test="$FUTURE_FLAG!=''">
                  <xsl:value-of select="concat($varYear,$varMonthCode)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="concat($varMonthCode,$varYear)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="varUnderlying">
              <xsl:choose>
                <xsl:when test="$PRANA_ROOT_NAME!=''">
                  <xsl:value-of select="$PRANA_ROOT_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$PB_ROOT_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

			  <xsl:variable name="Underlying">
				  <xsl:choose>
					  <xsl:when test="$PRANA_ROOT_NAME!=''">
						  <xsl:value-of select="$PRANA_ROOT_NAME"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$PB_ROOT_NAME"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name="FutureOptionSymbol">
				  <xsl:value-of select="translate(COL6,' ','')"/>
			  </xsl:variable>

			  <xsl:variable name="Future">
              <!--<xsl:choose>
								<xsl:when test="string-length(normalize-space(COL46)) &lt; 3">
									<xsl:value-of select="normalize-space(concat($varUnderlying,' ',substring(COL46,1,2),' ',$FUTURE_SUFFIX_NAME))"/>
								</xsl:when>
								<xsl:otherwise>-->
              <xsl:value-of select="normalize-space(concat($varUnderlying,' ',$MonthYearCode,$FUTURE_SUFFIX_NAME))"/>
              <!--</xsl:otherwise>
							</xsl:choose>-->
            </xsl:variable>
			  <xsl:variable name="Asset" select="COL4"/>

            <xsl:variable name="Symbol" select="normalize-space(COL6)"/>

            <xsl:variable name="StrikePrice" select="COL21"/>

            <Symbol>
				<xsl:choose>
					<xsl:when test="$PRANA_SYMBOL_NAME!=''">
						<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
					</xsl:when>

					<xsl:when test="$Asset='OPT'">
						<xsl:call-template name="Option">
							<xsl:with-param name="Symbol" select="normalize-space(COL6)"/>
						</xsl:call-template>
					</xsl:when>

					<xsl:when test="$Asset='FUT'">
						<xsl:value-of select="concat($Underlying,' ',substring(COL6,3))"/>
					</xsl:when>


					<xsl:when test="$Asset='FOP'">
						<xsl:value-of select="concat($Underlying,' ',substring($FutureOptionSymbol,3))"/>
					</xsl:when>
					<xsl:when test="$Asset='FSFOP'">
						<xsl:value-of select="concat($Underlying,' ',substring($FutureOptionSymbol,3))"/>
					</xsl:when>

					<xsl:when test="$Asset='STK'">
						<xsl:value-of select="$Symbol"/>
					</xsl:when>

					<xsl:when test="$Asset='CASH'">
						<xsl:value-of select="$Symbol"/>
					</xsl:when>

					<xsl:when test="COL6!='*'">
						<xsl:value-of select="normalize-space(concat(COL6,$PRANA_SUFFIX_NAME))"/>
					</xsl:when>

					<xsl:otherwise>
						<xsl:value-of select="$PB_SYMBOL_NAME"/>
					</xsl:otherwise>
				</xsl:choose>
			</Symbol>

			  <!--<IDCOOptionSymbol>
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:when test="$Asset='OPT'">
						  <xsl:value-of select="concat(COL6,'U')"/>
					  </xsl:when>

					  <xsl:when test="$Asset='FUT'">
						  <xsl:value-of select="''"/>
					  </xsl:when>


					  <xsl:when test="$Asset='FOP'">
						  <xsl:value-of select="''"/>
					  </xsl:when>
					  <xsl:when test="$Asset='FSFOP'">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:when test="$Asset='STK'">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:when test="$Asset='CASH'">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:when test="COL6!='*'">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </IDCOOptionSymbol>-->

			  <Volatility>
              <xsl:value-of select="0"/>
            </Volatility>

            <VolatilityUsed>
              <xsl:value-of select="'0'"/>
            </VolatilityUsed>

            <IntRateUsed>
              <xsl:value-of select="'0'"/>
            </IntRateUsed>

            <DividendUsed>
              <xsl:value-of select="'0'"/>
            </DividendUsed>

            <DeltaUsed>
              <xsl:value-of select="'0'"/>
            </DeltaUsed>

            <LastPriceUsed>
              <xsl:value-of select="'1'"/>
            </LastPriceUsed>

            <IntRate>
              <xsl:value-of select="0"/>
            </IntRate>

            <Dividend>
              <xsl:value-of select="0"/>
            </Dividend>

            <Delta>
              <xsl:value-of select="0"/>
            </Delta>

            <!--<LastPrice>
              <xsl:choose>
                <xsl:when  test="boolean(number($LastPrice))">
                  <xsl:value-of select="$LastPrice"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </LastPrice>-->



			  <xsl:variable name="Underlyer" select="substring(COL14,1,2)"/>

			  <xsl:variable name="Prana_Multiplier">
				  <xsl:value-of select ="document('../ReconMappingXML/PriceMulMapping.xml')/PriceMulMapping/PB[@Name=$PB_NAME]/MultiplierData[@PranaRoot=$Underlyer]/@Multiplier"/>
			  </xsl:variable>






			  <xsl:variable name="Cost">
				  <xsl:choose>
					  <xsl:when test="number($Prana_Multiplier)">
						  <xsl:value-of select="$LastPrice div $Prana_Multiplier"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$LastPrice"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>


			   <LastPrice>
              <xsl:choose>
                <xsl:when  test="boolean(number($Cost))">
                  <xsl:value-of select="$Cost"/>
                </xsl:when >
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose >
            </LastPrice>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>
