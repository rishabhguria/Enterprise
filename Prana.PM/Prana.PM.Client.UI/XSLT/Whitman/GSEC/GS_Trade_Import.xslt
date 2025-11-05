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

  <xsl:template name="Option">
    <xsl:param name="Symbol"/>
    <xsl:param name="Suffix"/>
    <xsl:if test="COL84='OPTION'">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before(substring-after($Symbol,' '),' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring-before(substring-after($Symbol,'/'),'/')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring-before(substring-after(substring-after($Symbol,' '),' '),'/')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring-before(substring-after(substring-after($Symbol,'/'),'/'),' ')"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-after(substring-after(substring-after($Symbol,'/'),'/'),' '),1,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring(substring-after(substring-after(substring-after($Symbol,'/'),'/'),' '),2),'#.00')"/>
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
      <xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
      <!--</xsl:otherwise>-->
      <!--

			</xsl:choose>-->
    </xsl:if>
  </xsl:template>

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//PositionMaster">


        <xsl:variable name="NetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL6"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($NetPosition) ">

          <PositionMaster>


			  <xsl:variable name="PB_NAME">
				  <xsl:value-of select="'GSEC'"/>
			  </xsl:variable>

			  <xsl:variable name="PB_SYMBOL_NAME">
				  <xsl:value-of select="COL72"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_SYMBOL_NAME">
				  <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
			  </xsl:variable>



			  <xsl:variable name="PB_YELLOW_NAME">
				  <xsl:value-of select="substring-after(COL26,' ')"/>
			  </xsl:variable>

			  <xsl:variable name="PB_ROOT_NAME">
				  <xsl:value-of select="substring(COL26,1,2)"/>
			  </xsl:variable>


			  <xsl:variable name ="PRANA_ROOT_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/Exchange[@Name=$PB_NAME]/SymbolData[@InstrmentCode = $PB_ROOT_NAME]/@UnderlyingCode"/>
			  </xsl:variable>
			  <xsl:variable name ="PRANA_ROOT_NAME1">
				  <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/Exchange[@Name=$PB_NAME]/SymbolData[@InstrmentCode = $PB_ROOT_NAME]/@ExchangeCode"/>
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

			  <xsl:variable name="ExchangeCode">
				  <xsl:choose>
					  <xsl:when test="$PRANA_ROOT_NAME1!=''">
						  <xsl:value-of select="$PRANA_ROOT_NAME1"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="''"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>
			  <!--<xsl:variable name ="FUTURE_EXCHANGE_CODE">
              <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME and @YellowFlag= $PB_YELLOW_NAME]/@ExchangeCode"/>
            </xsl:variable>-->

			  <xsl:variable  name="FUTURE_FLAG">
				  <xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME and @YellowFlag= $PB_YELLOW_NAME]/@ExpFlag"/>
			  </xsl:variable>

			  <xsl:variable name="MonthCode">
				  <xsl:value-of select="substring(normalize-space(COL26),3,1)"/>
			  </xsl:variable>


			  <xsl:variable name="Year" select="substring(normalize-space(COL26),4,1)"/>

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



			  <xsl:variable name="Future" select="concat($Underlying,' ',$MonthYearCode,$ExchangeCode)"/>


			  <xsl:variable name="Symbol" select="substring-before(COL26,' ')"/>
			  <Symbol>


				  <xsl:choose>
					  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
						  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
					  </xsl:when>


					  <xsl:when test="COL84='OPTION'">
						  <xsl:value-of select="''"/>

					  </xsl:when>

					  <xsl:when test="COL84='FUTURE'">
						  <xsl:value-of select="$Future"/>
					  </xsl:when>

					  <xsl:when test="COL84='FUTFOP'">
						  <xsl:value-of select="concat(substring(COL26,1,2),' ',substring(COL26,3,3),substring-before(substring-after(COL26,' '),' '))"/>
					  </xsl:when>

					  <xsl:when test="COL84='EQUITY'">
						  <xsl:value-of select="substring-before(COL26,' ')"/>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="$PB_SYMBOL_NAME"/>
					  </xsl:otherwise>

				  </xsl:choose>



			  </Symbol>

            <xsl:variable name="PB_FUND_NAME" select="COL1"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
			  <xsl:variable name="PB_CountnerParty" select="''"/>
			  <xsl:variable name="PRANA_CounterPartyID">
				  <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name= 'GS']/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
			  </xsl:variable>


			  <CounterPartyID>
				  <!--<xsl:choose>
									<xsl:when test="number($PRANA_CounterPartyID)">
										<xsl:value-of select="$PRANA_CounterPartyID"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>-->
				  <xsl:value-of select="'96'"/>
			  </CounterPartyID>

            <IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

              
                <xsl:when test="COL84='OPTION'">
                  <xsl:value-of select="concat(COL31,'U')"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>

              </xsl:choose>
            </IDCOOptionSymbol>

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$NetPosition&gt; 0">
                  <xsl:value-of select="$NetPosition"/>
                </xsl:when>
                <xsl:when test="$NetPosition">
                  <xsl:value-of select="$NetPosition* (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

			  <xsl:variable name="Underlyer" select="substring(COL26,1,2)"/>

			  <xsl:variable name="Prana_Multiplier">
				  <xsl:value-of select ="document('../ReconMappingXML/PriceMulMapping.xml')/PriceMulMapping/PB[@Name=$PB_NAME]/MultiplierData[@PranaRoot=$Underlyer]/@Multiplier"/>
			  </xsl:variable>
			  
			  

            <xsl:variable name="CostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL7"/>
              </xsl:call-template>
            </xsl:variable>

			  <xsl:variable name="Cost">
				  <xsl:choose>
					  <xsl:when test="number($Prana_Multiplier)">
						  <xsl:value-of select="$CostBasis div $Prana_Multiplier"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="$CostBasis"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>
			  
			  

            <CostBasis>
              <xsl:choose>
                <xsl:when test="$Cost &gt; 0">
                  <xsl:value-of select="$Cost"/>

                </xsl:when>
                <xsl:when test="$Cost &lt; 0">
                  <xsl:value-of select="$Cost * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </CostBasis>

			  <xsl:variable name="Commission">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL91"/>
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
					  <xsl:with-param name="Number" select="COL15"/>
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

			  <xsl:variable name="OtherFee">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL18"/>
				  </xsl:call-template>
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

						<xsl:variable name="Side" select="COL2"/>

            <SideTagValue>
              <xsl:choose>

                <xsl:when test="COL84='OPTION'">
                  <xsl:choose>
                    <xsl:when test="$Side='BUY'">
                      <xsl:value-of select="'A'"/>
                    </xsl:when>

                    <xsl:when test="$Side='SELL'">
                      <xsl:value-of select="'D'"/>
                    </xsl:when>


                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>


                  </xsl:choose>
                </xsl:when>



                <xsl:otherwise>


                  <xsl:choose>
                    <xsl:when test="$Side='BUY'">
                      <xsl:value-of select="'1'"/>
                    </xsl:when>

                    <xsl:when test="$Side='SELL'">
                      <xsl:value-of select="'2'"/>
                    </xsl:when>



                    <xsl:otherwise>
                      <xsl:value-of select="''"/>
                    </xsl:otherwise>


                  </xsl:choose>


                </xsl:otherwise>

              </xsl:choose>

            </SideTagValue>

            <!--<SEDOL>
              <xsl:value-of select="COL9" />
            </SEDOL>-->

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

            <PositionStartDate>
              <xsl:value-of select="COL3"/>
            </PositionStartDate>

			  <PositionSettlementDate>
				  <xsl:value-of select="COL4"/>
			  </PositionSettlementDate>

            <!--<TradeAttribute1>
              <xsl:choose>
                <xsl:when test="contains(translate(COL36,$lower_CONST,$upper_CONST),'SWAP')">
                  <xsl:choose>
                    <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                      <xsl:value-of select="concat(translate($PRANA_SYMBOL_NAME,$upper_CONST,$lower_CONST),'_swap')"/>
                    </xsl:when>

                    <xsl:when test="COL36!='*'">
                      <xsl:value-of select="concat(translate(substring-before(COL36,' '),$upper_CONST,$lower_CONST),'_swap')"/>
                    </xsl:when>

                    <xsl:otherwise>
                      <xsl:value-of select="concat(translate($PB_SYMBOL_NAME,$upper_CONST,$lower_CONST),'_swap')"/>
                    </xsl:otherwise>

                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>

                  <xsl:choose>
                    <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                      <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                    </xsl:when>

                    <xsl:when test="COL36!='*'">
                      <xsl:value-of select="substring-before(COL36,' ')"/>
                    </xsl:when>
r
                    <xsl:otherwise>
                      <xsl:value-of select="$PB_SYMBOL_NAME"/>
                    </xsl:otherwise>

                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </TradeAttribute1>-->

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>