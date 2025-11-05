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
      <xsl:when test="$varMonth=1">
        <xsl:value-of select="'F'"/>
      </xsl:when>
      <xsl:when test="$varMonth=2">
        <xsl:value-of select="'G'"/>
      </xsl:when>
      <xsl:when test="$varMonth=3">
        <xsl:value-of select="'H'"/>
      </xsl:when>
      <xsl:when test="$varMonth=4">
        <xsl:value-of select="'J'"/>
      </xsl:when>
      <xsl:when test="$varMonth=5">
        <xsl:value-of select="'K'"/>
      </xsl:when>
      <xsl:when test="$varMonth=6">
        <xsl:value-of select="'M'"/>
      </xsl:when>
      <xsl:when test="$varMonth=7">
        <xsl:value-of select="'N'"/>
      </xsl:when>
      <xsl:when test="$varMonth=8">
        <xsl:value-of select="'Q'"/>
      </xsl:when>
      <xsl:when test="$varMonth=9">
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

  <!--<xsl:template name="revMonthCode">
		<xsl:param name="varRevMonth"/>
		<xsl:choose>
			<xsl:when test="$varRevMonth='F'">
				<xsl:value-of select="1"/>
			</xsl:when>
			<xsl:when test="$varRevMonth='G'">
				<xsl:value-of select="2"/>
			</xsl:when>
			<xsl:when test="$varRevMonth='H'">
				<xsl:value-of select="3"/>
			</xsl:when>
			<xsl:when test="$varRevMonth='J'">
				<xsl:value-of select="4"/>
			</xsl:when>
			<xsl:when test="$varRevMonth='K'">
				<xsl:value-of select="5"/>
			</xsl:when>
			<xsl:when test="$varRevMonth='M'">
				<xsl:value-of select="6"/>
			</xsl:when>
			<xsl:when test="$varRevMonth='N'">
				<xsl:value-of select="7"/>
			</xsl:when>
			<xsl:when test="$varRevMonth='Q'">
				<xsl:value-of select="8"/>
			</xsl:when>
			<xsl:when test="$varRevMonth='U'">
				<xsl:value-of select="9"/>
			</xsl:when>
			<xsl:when test="$varRevMonth='V'">
				<xsl:value-of select="10"/>
			</xsl:when>
			<xsl:when test="$varRevMonth='X'">
				<xsl:value-of select="11"/>
			</xsl:when>
			<xsl:when test="$varRevMonth='Z'">
				<xsl:value-of select="12"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>-->

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">

        <xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL7"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Position)">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Bank of America Merrill Lynch'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL40)"/>
            </xsl:variable>

			  <xsl:variable name="PRANA_SYMBOL_NAME">
				  <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
			  </xsl:variable>



			  <!--<xsl:variable name="Underlying">
							<xsl:value-of select="string-length(substring-before(COL24,' '))-2"/>
						</xsl:variable>-->

            <xsl:variable name="PB_ROOT_NAME">
              <xsl:value-of select="normalize-space(substring(COL46,1,2))"/>
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
              <xsl:value-of select="normalize-space(COL48)"/>
            </xsl:variable>

            <!--<xsl:variable name="PB_EXCHANGE_NAME">
							<xsl:value-of select="normalize-space(COL91)"/>
						</xsl:variable>-->

            <xsl:variable name ="PRANA_ROOT_NAME">
              <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode = $PB_ROOT_NAME and @YellowFlag = $PB_YELLOW_NAME]/@UnderlyingCode"/>
            </xsl:variable>

            <xsl:variable name ="FUTURE_EXCHANGE_CODE">
              <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME and @YellowFlag= $PB_YELLOW_NAME]/@ExchangeCode"/>
            </xsl:variable>

            <xsl:variable  name="FUTURE_FLAG">
              <xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME and @YellowFlag= $PB_YELLOW_NAME]/@ExpFlag"/>
            </xsl:variable>

            <xsl:variable name="MonthCode">
              <!--<xsl:call-template name="MonthCode">
								<xsl:with-param name="varMonth" select="number(substring-before(substring-after(normalize-space(COL18),'/'),'/'))"/>
							</xsl:call-template>-->
              <xsl:value-of select="substring(normalize-space(COL46),3,1)"/>
            </xsl:variable>

            <xsl:variable name="Year" select="substring(normalize-space(COL46),4,1)"/>

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

            <xsl:variable name="Symbol">
              <xsl:value-of select="concat(normalize-space(COL46),' ',normalize-space(COL48))"/>
            </xsl:variable>

            <Symbol>


              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>






                <xsl:when test="$Symbol!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>

            </Symbol>

			  <Bloomberg>
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="''"/>
					  </xsl:when>
					  <xsl:when test="$Symbol!='*'">
						  <xsl:value-of select="$Symbol"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Bloomberg>
			  <Symbology>
				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="'Symbol'"/>
					  </xsl:when>
					  <xsl:when test="$Symbol!='*'">
						  <xsl:value-of select="'Bloomberg'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </Symbology>
            <xsl:variable name="PB_FUND_NAME" select="normalize-space(COL4)"/>
			  <xsl:variable name ="PRANA_FUND_NAME">
				  <xsl:value-of select ="document('../../../MappingFiles/ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
			  </xsl:variable>


			  <FundName>
              <xsl:choose>

                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>

              </xsl:choose>
            </FundName>

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$Position &gt; 0">
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

            <!--<xsl:variable name="COL11">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL11"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="COL9">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL9"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="COL8">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL8"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="CostBasis">
							<xsl:choose>
								<xsl:when test="number($COL9)">
									<xsl:value-of select="$COL9 div $COL8"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>-->

            <!--<xsl:variable name="CostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL7"/>
							</xsl:call-template>
						</xsl:variable>-->

			  <xsl:variable name="PB_Contract" select="COL9"/>

            <xsl:variable name="PRANA_Multiplier">
              <xsl:value-of select ="document('../../../MappingFiles/ReconMappingXml/MultiplierMapping.xml')/SymbolMultiplierMapping/PB[@Name=$PB_NAME]/MultiplierData[@Contract=$PB_Contract]/@PBMultiplier"/>
            </xsl:variable>

			  <xsl:variable name="PRANA_MultiplierToExclude">
				  <xsl:value-of select ="document('../../../MappingFiles/ReconMappingXml/MultiplierMappingToExclude.xml')/SymbolMultiplierMapping/PB[@Name=$PB_NAME]/MultiplierData[@Contract=$PB_Contract]/@PBMultiplier"/>
			  </xsl:variable>

            <xsl:variable name="Price">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL16"/>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="CostBasis">
				
              <xsl:choose>
				  <xsl:when test="$PRANA_MultiplierToExclude!=''">
					  <xsl:value-of select="$Price"/>
				  </xsl:when>
                <xsl:when test="$PRANA_Multiplier!=''">
                  <xsl:value-of select="format-number($Price * $PRANA_Multiplier,'#.######')"/>
					<!--<xsl:value-of select="$Price * $PRANA_Multiplier"/>-->
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select='number($Price)'/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <CostBasis>
              <xsl:choose>
                <xsl:when test="$CostBasis &gt; 0">
                  <xsl:value-of select="$CostBasis"/>

                </xsl:when>
                <xsl:when test="$CostBasis &lt; 0">
                  <xsl:value-of select="$CostBasis * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </CostBasis>

            <xsl:variable name="Side" select="normalize-space(COL6)"/>

            <SideTagValue>
              <xsl:choose>

                <xsl:when test="$Side= '1'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>

                <xsl:when test="$Side = '2'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </SideTagValue>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>



            <!--<PositionStartDate>
							<xsl:choose>
								<xsl:when test="contains(normalize-space(COL18),' ')">
									<xsl:value-of select="substring-before(normalize-space(COL18),' ')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="normalize-space(COL18)"/>
								</xsl:otherwise>
							</xsl:choose>
						</PositionStartDate>-->

            <xsl:variable name="TradeDateDay" select="substring(COL5,7,2)"/>
            <xsl:variable name="TradeDateMonth" select="substring(COL5,5,2)"/>
            <xsl:variable name="TradeDateYear" select="substring(COL5,1,4)"/>



            <PositionStartDate>
              <xsl:value-of select="concat($TradeDateMonth,'/',$TradeDateDay,'/',$TradeDateYear)"/>
            </PositionStartDate>


			  <xsl:variable name="TradeDateDay1" select="substring(COL1,7,2)"/>
			  <xsl:variable name="TradeDateMonth1" select="substring(COL1,5,2)"/>
			  <xsl:variable name="TradeDateYear1" select="substring(COL1,1,4)"/>



			  <PositionSettlementDate>
				  <xsl:value-of select="concat($TradeDateMonth1,'/',$TradeDateDay1,'/',$TradeDateYear1)"/>
			  </PositionSettlementDate>


            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL21"/>
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

            <xsl:variable name="StampDuty">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL25"/>
              </xsl:call-template>
            </xsl:variable>

            <StampDuty>
              <xsl:choose>
                <xsl:when test="$StampDuty &gt; 0">
                  <xsl:value-of select="$StampDuty"/>

                </xsl:when>
                <xsl:when test="$StampDuty &lt; 0">
                  <xsl:value-of select="$StampDuty * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </StampDuty>

            <xsl:variable name="COL22">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL22"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="COL23">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL23"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="COL24">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL24"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="COL26">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL26"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="OtherFee">
              <xsl:value-of select="number($COL22 + $COL23 + $COL24 + $COL26)"/>
            </xsl:variable>

            <Fees>
              <xsl:choose>
                <xsl:when test="$OtherFee &gt; 0">
                  <xsl:value-of select="$OtherFee"/>

                </xsl:when>
                <xsl:when test="$OtherFee &lt; 0">
                  <xsl:value-of select="$OtherFee * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </Fees>

			  <xsl:variable name="Multiplier" select="COL70"/>

			  <Multiplier>

				  <xsl:choose>
					  <xsl:when test="$PRANA_MultiplierToExclude!=''">
						  <xsl:value-of select="$PRANA_MultiplierToExclude"/>
					  </xsl:when>
					  <xsl:when test="$PRANA_Multiplier!=''">
						  <xsl:value-of select="$Multiplier div $PRANA_Multiplier"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$Multiplier"/>
					  </xsl:otherwise>
				  </xsl:choose>
				 
			  </Multiplier>

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>