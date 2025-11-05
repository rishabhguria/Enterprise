<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

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

  </xsl:template>


  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month=01 ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=02 ">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=03 ">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=04 ">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=05 ">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test="$Month=06 ">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=07  ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=08  ">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=09 ">
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
        <xsl:when test="$Month=01 ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=02 ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=03 ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=04 ">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=05 ">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=06 ">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=07  ">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=08  ">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=09 ">
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


  <xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:param name="Suffix"/>
    <xsl:if test="contains(COL7,'CALL') or contains(COL7,'PUT')">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before($Symbol,'1')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),5,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),3,2)"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),1,2)"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),7,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring(substring-after($Symbol,$UnderlyingSymbol),8) div 1000  ,'#.00')"/>
      </xsl:variable>


      <xsl:variable name="MonthCodeVar">
        <xsl:call-template name="MonthCode">
          <xsl:with-param name="Month" select="number($ExpiryMonth)"/>
          <xsl:with-param name="PutOrCall" select="$PutORCall"/>
        </xsl:call-template>
        <!--<xsl:value-of select="substring(substring-after($Symbol,$UnderlyingSymbol),5,1)"/>-->
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
      <xsl:value-of select="concat('O:',$UnderlyingSymbol,'',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
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

  <!--<xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:param name="Suffix"/>
    <xsl:if test="COL18='Option'">

      -->
  <!--</xsl:otherwise>-->
  <!--
      -->
  <!--

			</xsl:choose>-->
  <!--
    </xsl:if>-->
  <!--
  </xsl:template>-->

  <xsl:template match="/">


    <DocumentElement>

      <xsl:for-each select ="//Comparision">


        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL7"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity) and not(contains(COL6, 'FADXX'))">

          <PositionMaster>
            <xsl:variable name ="varPBName">
              <xsl:value-of select ="'BTIG'"/>
            </xsl:variable>
            <xsl:variable name ="PB_Account_NAME">
              <xsl:value-of select ="COL1"/>
            </xsl:variable>
            <xsl:variable name="PRANA_Account_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/AccountMapping/PB[@Name=$varPBName]/AccountData[@PBAccountName=$PB_Account_NAME]/@PranaAccount"/>
            </xsl:variable>
            <AccountName>
              <xsl:choose>
                <xsl:when test ="$PRANA_Account_NAME!=''">
                  <xsl:value-of select ="$PRANA_Account_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$PB_Account_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>

			  <xsl:variable name="Asset">
				  <xsl:choose>
					  <xsl:when test="string-length(COL5) &gt; 20">
						  <xsl:value-of select="'Option'"/>
					  </xsl:when>
				  </xsl:choose>

			  </xsl:variable>



			  <xsl:variable name ="Symbol">
              <xsl:value-of select ="COL5"/>
            </xsl:variable>
            <xsl:variable name ="PB_COMPANY">

              <xsl:value-of select ="COL6"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL">

              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
            </xsl:variable>

            <Symbol>
				<xsl:choose>
					<xsl:when test="$PRANA_SYMBOL!=''">
						<xsl:value-of select="$PRANA_SYMBOL"/>
					</xsl:when>

					<xsl:when test="$Asset='Option'">
						<xsl:value-of select="''"/>
					</xsl:when>

					<xsl:when test="string-length(COL5)=7 or string-length(COL5) &gt; 7">
						<xsl:value-of select="''"/>
					</xsl:when>

					<xsl:when test="COL5!='*'">
						<xsl:value-of select="COL5"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$PB_COMPANY"/>
					</xsl:otherwise>
				</xsl:choose>
            </Symbol>

			  <IDCOOptionSymbol>
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL!=''">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:when test="$Asset='Option'">
						  <xsl:value-of select="concat(COL5,'U')"/>
					  </xsl:when>

					  <xsl:when test="string-length(COL5)=7 or string-length(COL5) &gt; 7">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:when test="COL5!='*'">
						  <xsl:value-of select="''"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </IDCOOptionSymbol>

			  <SEDOL>

				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL!=''">
						  <xsl:value-of select="''"/>
					  </xsl:when>

					  <xsl:when test="$Asset='Option'">
						  <xsl:value-of select="''"/>
					  </xsl:when>


					  <xsl:when test="string-length(COL5)=7 or string-length(COL5) &gt; 7">
						  <xsl:value-of select="COL5"/>
					  </xsl:when>
					  <xsl:when test="COL5!='*'">
						  <xsl:value-of select="''"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>

			  </SEDOL>





			  <Quantity>
              <xsl:choose>
                <xsl:when test="number($Quantity)">
                  <xsl:value-of select="$Quantity"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>





            <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL8"/>
              </xsl:call-template>
            </xsl:variable>


            <AvgPX>
              <xsl:choose>
                <xsl:when test="$AvgPrice &gt; 0">
                  <xsl:value-of select="$AvgPrice"/>

                </xsl:when>
                <xsl:when test="$AvgPrice &lt; 0">
                  <xsl:value-of select="$AvgPrice * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </AvgPX>



            <xsl:variable name="Side" select="COL4"/>

            <Side>
				<xsl:choose>
					<xsl:when test="$Asset='Option'">
						<xsl:choose>
							<xsl:when test="COL4='BUY'">
								<xsl:value-of select="'Buy to Open'"/>
							</xsl:when>

							<xsl:when test="COL4='SEL'">
								<xsl:value-of select="'Sell to Close'"/>
							</xsl:when>
							<xsl:when test="COL4='SSL'">
								<xsl:value-of select="'Sell to Open'"/>
							</xsl:when>
							<xsl:when test="COL4='BTC'">
								<xsl:value-of select="'Buy to Close'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>

					<xsl:otherwise>

						<xsl:choose>
							<xsl:when test="COL4='BUY'">
								<xsl:value-of select="'Buy'"/>
							</xsl:when>

							<xsl:when test="COL4='SEL'">
								<xsl:value-of select="'Sell'"/>
							</xsl:when>
							<xsl:when test="COL4='SSL'">
								<xsl:value-of select="'Sell Short'"/>
							</xsl:when>
							<xsl:when test="COL4='BTC'">
								<xsl:value-of select="'Buy to Close'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>



				</xsl:choose>
            </Side>






            <PBSymbol>
              <xsl:value-of select="$PB_COMPANY"/>


            </PBSymbol>



            <xsl:variable name ="Month">
              <xsl:value-of select ="substring(COL7,5,2)"/>
            </xsl:variable>
            <xsl:variable name ="Date">
              <xsl:value-of select ="substring(COL7,7,2)"/>
            </xsl:variable>
            <xsl:variable name ="Year">
              <xsl:value-of select ="substring(COL7,1,4)"/>
            </xsl:variable>

            <TradeDate>
              <xsl:value-of select ="COL2"/>

            </TradeDate>

			  <SettlementDate>
				  <xsl:value-of select="COL3"/>
			  </SettlementDate>



            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL9"/>
              </xsl:call-template>
            </xsl:variable>

            <Commission>
              <xsl:choose>
                <xsl:when test =" $Commission &gt; 0">
                  <xsl:value-of select ="$Commission"/>
                </xsl:when>
                <xsl:when test ="$Commission &lt; 0">
                  <xsl:value-of select ="$Commission * -1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

			  <OtherBrokerFees>
				  <xsl:choose>
					  <xsl:when test="contains(COL4,'BUY') or contains(COL4,'BTC')">
						  <xsl:value-of select="COL13"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="'0'"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </OtherBrokerFees>

            <SecFee>
              <xsl:choose>

                <xsl:when test="COL4='SEL' or COL4='SSL'">
                  <xsl:value-of select="COL10"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'0'"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecFee>


			  <xsl:variable name="TotalCommissionandFees" select="COL9 + COL10"/>
			  <TotalCommissionandFees>
				  <xsl:choose>
					  <xsl:when test="$TotalCommissionandFees &gt; 0">
						  <xsl:value-of select="$TotalCommissionandFees"/>
					  </xsl:when>
					  <xsl:when test="$TotalCommissionandFees &lt; 0">
						  <xsl:value-of select="$TotalCommissionandFees*-1"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </TotalCommissionandFees>


			  <xsl:variable name="GrossNotionalValue">
				  <xsl:choose>
					  <xsl:when test="$Asset='Option'">
						  <xsl:value-of select="format-number(translate(COL7,',','') * translate(COL8,',','') * 100, '#.##')"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="format-number(translate(COL7,',','') * translate(COL8,',',''), '#.##')"/>
					  </xsl:otherwise>
				  </xsl:choose>
				 
			  </xsl:variable>



			  <GrossNotionalValue>
				  <xsl:choose>
					  <xsl:when test="$GrossNotionalValue &gt; 0">
						  <xsl:value-of select="$GrossNotionalValue"/>
					  </xsl:when>
					  <xsl:when test="$GrossNotionalValue &lt; 0">
						  <xsl:value-of select="$GrossNotionalValue*(-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </GrossNotionalValue>

            <xsl:variable name="NetNotional">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL14"/>
              </xsl:call-template>
            </xsl:variable>


			  <NetNotionalValue>
              <xsl:choose>
                <xsl:when test =" $NetNotional &gt; 0">
                  <xsl:value-of select ="$NetNotional"/>
                </xsl:when>
                <xsl:when test ="$NetNotional &lt; 0">
                  <xsl:value-of select ="$NetNotional * -1"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>

            <xsl:variable name="NetNotional1">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL14"/>
              </xsl:call-template>
            </xsl:variable>


			  <NetNotionalValueBase>
				  <xsl:choose>
					  <xsl:when test =" $NetNotional1 &gt; 0">
						  <xsl:value-of select ="$NetNotional1"/>
					  </xsl:when>
					  <xsl:when test ="$NetNotional1 &lt; 0">
						  <xsl:value-of select ="$NetNotional1 * -1"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </NetNotionalValueBase>

            <BaseCurrency>
              <xsl:value-of select="COL15"/>
            </BaseCurrency>

            <SettlCurrency>
              <xsl:value-of select="COL15"/>
            </SettlCurrency>

            <SMRequest>
              <xsl:value-of select="'true'"/>
            </SMRequest>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>