<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
  <xsl:output method="xml" indent="yes"/>

  <msxsl:script language="C#" implements-prefix="my">
    public string Now(int year, int month)
    {
    DateTime firstWednesday= new DateTime(year, month, 1);
    while (firstWednesday.DayOfWeek != DayOfWeek.Wednesday)
    {
    firstWednesday = firstWednesday.AddDays(1);
    }
    return firstWednesday.ToString();
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
    <xsl:param name="varMonth"/>
    <xsl:choose>
      <xsl:when test="$varMonth=01">
        <xsl:value-of select="'F'"/>
      </xsl:when>
      <xsl:when test="$varMonth=02">
        <xsl:value-of select="'G'"/>
      </xsl:when>
      <xsl:when test="$varMonth=03">
        <xsl:value-of select="'H'"/>
      </xsl:when>
      <xsl:when test="$varMonth=04">
        <xsl:value-of select="'J'"/>
      </xsl:when>
      <xsl:when test="$varMonth=05">
        <xsl:value-of select="'K'"/>
      </xsl:when>
      <xsl:when test="$varMonth=06">
        <xsl:value-of select="'M'"/>
      </xsl:when>
      <xsl:when test="$varMonth=07">
        <xsl:value-of select="'N'"/>
      </xsl:when>
      <xsl:when test="$varMonth=08">
        <xsl:value-of select="'Q'"/>
      </xsl:when>
      <xsl:when test="$varMonth=09">
        <xsl:value-of select="'U'"/>
      </xsl:when>
      <xsl:when test="$varMonth=10">
        <xsl:value-of select="'V'"/>
      </xsl:when>
      <xsl:when test="$varMonth=11">
        <xsl:value-of select="'X'"/>
      </xsl:when>
      <xsl:when test="$varMonth=12">
        <xsl:value-of select="'Z'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//Comparision">


        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL12"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity) ">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'MS'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL34)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

           <xsl:variable name="Asset">
            <xsl:choose>
              <xsl:when test="COL39='FUTURE'">
                <xsl:value-of select="'Future'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'FutureOption'"/>
              </xsl:otherwise>
            </xsl:choose>
            </xsl:variable>


            <xsl:variable name="PB_ROOT_NAME">
              <xsl:value-of select="normalize-space(COL36)"/>
            </xsl:variable>

            <xsl:variable name="PB_YELLOW_NAME">
              <!--<xsl:value-of select="normalize-space()"/>-->
              <!--<xsl:choose>
								<xsl:when test ="contains(substring-after(normalize-space(COL20),' '),' ')">
									<xsl:value-of select="substring-after(substring-after(normalize-space(COL20),' '),' ')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-after(normalize-space(COL20),' ')"/>
								</xsl:otherwise>
							</xsl:choose>-->
              <xsl:value-of select="''"/>
            </xsl:variable>

            <!--<xsl:variable name="PB_EXCHANGE_NAME">
							<xsl:value-of select="normalize-space(COL91)"/>
						</xsl:variable>-->

            <xsl:variable name ="PRANA_ROOT_NAME">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode = $PB_ROOT_NAME and @YellowFlag = $PB_YELLOW_NAME]/@UnderlyingCode"/>
            </xsl:variable>

            <xsl:variable name ="FUTURE_EXCHANGE_CODE">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME and @YellowFlag= $PB_YELLOW_NAME]/@ExchangeCode"/>
            </xsl:variable>

            <xsl:variable  name="FUTURE_FLAG">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME and @YellowFlag= $PB_YELLOW_NAME]/@ExpFlag"/>
            </xsl:variable>

            <xsl:variable name="MonthCode">
              <xsl:call-template name="MonthCode">
                <xsl:with-param name="varMonth" select="number(COL8)"/>
              </xsl:call-template>
              <!--<xsl:value-of select="substring(COL15,5,2)"/>-->
            </xsl:variable>

            <xsl:variable name="Year" select="substring(COL9,4,1)"/>

            <xsl:variable name="MonthYearCode">
              <xsl:choose>
                <xsl:when test="$FUTURE_FLAG!=''">
                  <xsl:value-of select="concat($Year,$MonthCode)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="concat($MonthCode,$Year)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="Underlying">
              <xsl:choose>
                <xsl:when test="$PRANA_ROOT_NAME!=''">
                  <xsl:value-of select="translate($PRANA_ROOT_NAME,$lower_CONST,$upper_CONST)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="translate($PB_ROOT_NAME,$lower_CONST,$upper_CONST)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name ="PRANA_Strike_Multiplier">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@UnderlyingCode = $Underlying]/@StrikeMul"/>
            </xsl:variable>



            <!--<xsl:variable name="StrikePrice">

              <xsl:choose>
                <xsl:when test="number($PRANA_Strike_Multiplier)">
                  <xsl:value-of select="COL10 * $PRANA_Strike_Multiplier"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL10"/>
                </xsl:otherwise>
              </xsl:choose>
              
            </xsl:variable>-->

			  <xsl:variable name="StrikePrice">

				  <xsl:value-of select="substring-before(substring-after(normalize-space(COL49),' '),' ')"/>
				  
			  </xsl:variable>

				  <xsl:variable name="Currency">
				  <xsl:value-of select="substring-after(substring-after(substring-after(translate(COL49,$lower_CONST,$upper_CONST),' '),' '),' ')"/>
			  </xsl:variable>

			  <xsl:variable name="Bloomberg">
				  <xsl:choose>
					  <!--<xsl:when test="contains(COL39,'ELEC')">
						  <xsl:value-of select="normalize-space(translate(COL39,'ELEC',''))"/>
					  </xsl:when>-->

					  <xsl:when test="contains(substring-before(substring-after(COL49,' '),' '),'ELEC')">
						  <xsl:value-of select="translate(concat(substring-before(COL49,' '),' ',substring-after(substring-after(COL49,' '),' ')),$lower_CONST,$upper_CONST)"/>
					  </xsl:when>
					  <xsl:when test="contains(substring-before(substring-after(substring-after(COL49,' '),' '),' '),'ELEC')">
						  <xsl:value-of select="translate(concat(substring-before(COL49,' '),' ',substring-before(substring-after(COL49,' '),' '),' ',substring-after(substring-after(substring-after(COL49,' '),' '),' ')),$lower_CONST,$upper_CONST)"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="translate(COL49,$lower_CONST,$upper_CONST)"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

            <Symbol>

              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>	

                <xsl:when test="$Asset='Future'">
					<xsl:value-of select="$Bloomberg"/>
                </xsl:when>
				
                
                 <!--<xsl:when test="$Asset='FutureOption'">
                  <xsl:value-of select="concat($Underlying,$MonthYearCode,COL11,' ',$StrikePrice,' ',$Currency)"/>
                </xsl:when>-->


                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>

              </xsl:choose>


            </Symbol>

			  <Bloomberg>
				 
				  <xsl:value-of select="''"/>
			  </Bloomberg>

			  <Multiplier>
				  <xsl:value-of select="COL53"/>
			  </Multiplier>



            <xsl:variable name="PB_FUND_NAME" select="COL4"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <AccountName>
              <xsl:choose>

                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </AccountName>


            <Quantity>
              <xsl:choose>
                <xsl:when test="$Quantity &gt; 0">
                  <xsl:value-of select="$Quantity"/>

                </xsl:when>
                <xsl:when test="$Quantity &lt; 0">
                  <xsl:value-of select="$Quantity * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </Quantity>





            <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL15"/>
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




            <xsl:variable name="Side" select="COL13"/>
            <Side>
              <xsl:choose>
                <xsl:when test="$Side='B'">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>

                <xsl:when test="$Side='S'">
                  <xsl:value-of select="'Sell'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>


              </xsl:choose>

            </Side>







            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>


            </PBSymbol>


            <xsl:variable name ="Date" select="COL5"/>


            <xsl:variable name="Year1" select="substring($Date,1,4)"/>
            <xsl:variable name="Month" select="substring($Date,5,2)"/>
            <xsl:variable name="Day" select="substring($Date,7,2)"/>



            <TradeDate>

              <xsl:value-of select="concat($Month,'/',$Day,'/',$Year1)"/>

            </TradeDate>

            <CurrencySymbol>
              <xsl:value-of select="COL14"/>
            </CurrencySymbol>

			  <SettlCurrency>
				  <xsl:value-of select="COL14"/>
			  </SettlCurrency>

            <!--<Bloomberg>
              <xsl:value-of select="COL49"/>
            </Bloomberg>-->



            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL26"/>
              </xsl:call-template>
            </xsl:variable>

            <Commission>

              <xsl:choose>

                <xsl:when test="$Commission &gt; 0">
                  <xsl:value-of select="$Commission"/>
                </xsl:when>

                <xsl:when test="$Commission &lt; 0">
                  <xsl:value-of select="$Commission * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>

            </Commission>

            <xsl:variable name="TaxOnCommissions">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL43"/>
              </xsl:call-template>
            </xsl:variable>

            <TaxOnCommissions>

              <xsl:choose>

                <xsl:when test="$TaxOnCommissions &gt; 0">
                  <xsl:value-of select="$TaxOnCommissions"/>
                </xsl:when>

                <xsl:when test="$TaxOnCommissions &lt; 0">
                  <xsl:value-of select="$TaxOnCommissions * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>

            </TaxOnCommissions>


            <xsl:variable name="Fees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL29"/>
              </xsl:call-template>
            </xsl:variable>


            <Fees>
              <xsl:choose>
                <xsl:when test="$Fees &gt; 0">
                  <xsl:value-of select="$Fees"/>

                </xsl:when>
                <xsl:when test="$Fees &lt; 0">
                  <xsl:value-of select="$Fees * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </Fees>


            <xsl:variable name="ClearingFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL46"/>
              </xsl:call-template>
            </xsl:variable>


            <ClearingFee>
              <xsl:choose>
                <xsl:when test="$ClearingFee &gt; 0">
                  <xsl:value-of select="$ClearingFee"/>

                </xsl:when>
                <xsl:when test="$ClearingFee &lt; 0">
                  <xsl:value-of select="$ClearingFee * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </ClearingFee>

            <xsl:variable name="GrossNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL62"/>
              </xsl:call-template>
            </xsl:variable>

			  <GrossNotionalValue>

				  <xsl:choose>

					  <xsl:when test="$GrossNotionalValue &gt; 0">
						  <xsl:value-of select="$GrossNotionalValue"/>
					  </xsl:when>

					  <xsl:when test="$GrossNotionalValue &lt; 0">
						  <xsl:value-of select="$GrossNotionalValue * (-1)"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>

				  </xsl:choose>

			  </GrossNotionalValue>

            <xsl:variable name="NetNotionalValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL71"/>
              </xsl:call-template>
            </xsl:variable>

            <NetNotionalValueBase>

              <xsl:choose>

                <xsl:when test="$NetNotionalValueBase &gt; 0">
                  <xsl:value-of select="$NetNotionalValueBase"/>
                </xsl:when>

                <xsl:when test="$NetNotionalValueBase &lt; 0">
                  <xsl:value-of select="$NetNotionalValueBase * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>

            </NetNotionalValueBase>


            <xsl:variable name="FXRate">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL69"/>
              </xsl:call-template>
            </xsl:variable>

            <FXRate>
              <xsl:choose>
                <xsl:when test="number($FXRate)">
                  <xsl:value-of select="$FXRate"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="1"/>
                </xsl:otherwise>
              </xsl:choose>
            </FXRate>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>