<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
  <xsl:output method="xml" indent="yes"/>

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
		<xsl:if test="contains(COL4,'CALL') or contains(COL4,'PUT')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(COL3,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(substring-after(normalize-space(COL3),' '),3,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(substring-after(normalize-space(COL3),' '),1,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(normalize-space(COL3),' '),7,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL3),' '),' '),' ')"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' ') div 1000  ,'#.00')"/>
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
			<!--<xsl:variable name="ThirdFriday">
				<xsl:choose>
					<xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
						<xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
					</xsl:when>
				</xsl:choose>
			</xsl:variable>-->
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

  <xsl:template match="/">

    <DocumentElement>

      <xsl:for-each select ="//Comparision">


        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL8"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($Quantity) ">

          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'Pershing'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="COL4"/>
            </xsl:variable>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>

			  <xsl:variable name="Asset">
				  <xsl:choose>
					  <xsl:when test="contains(COL4,'PUT')">
						  <xsl:value-of select="'Option'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="'Equity'"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

            <xsl:variable name="Symbol" select="COL3"/>
            <Symbol>

              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
				  
				  <xsl:when test="$Asset='Option'">
					  <xsl:call-template name="Option">
						  <xsl:with-param name="Symbol" select="COL3"/>
						  <xsl:with-param name="Suffix" select="''"/>
					  </xsl:call-template>
				  </xsl:when>

                <xsl:when test="$Symbol!=''">
                  <xsl:value-of select="$Symbol"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="$PB_SYMBOL_NAME"/>
                </xsl:otherwise>
              </xsl:choose>


            </Symbol>

            <xsl:variable name="TradeDate" select="COL1"/>

            <TradeDate>
              <xsl:value-of select="$TradeDate"/>
            </TradeDate>


            <!--<xsl:variable name="PB_FUND_NAME" select="COL2"/>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select ="document('../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>-->

            <!--<FundName>
              <xsl:choose>

                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
            </FundName>-->

			  <FundName>
				  <xsl:value-of select="'Makalu International Equity Master Fund LTD'"/>
			  </FundName>

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
                <xsl:with-param name="Number" select="COL12"/>
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


			  <xsl:variable name="Side" select="COL7"/>

            <Side>
				<xsl:choose>
					<xsl:when test="$Asset='Option'">
						<xsl:choose>
							<xsl:when test="$Side='BUY' ">
								<xsl:value-of select="'Buy to Open'"/>
							</xsl:when>
							<xsl:when test="$Side='S' or $Side='SELL' ">
								<xsl:value-of select="'Sell to Close'"/>
							</xsl:when>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="$Side='BUY' ">
								<xsl:value-of select="'Buy'"/>
							</xsl:when>

							<xsl:when test="$Side='S' or $Side='SELL' ">
								<xsl:value-of select="'Sell'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
            </Side>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>
            <SettlementDate>
							<xsl:value-of select="normalize-space(COL5)"/>
						</SettlementDate>

            <xsl:variable name="Comm">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL13"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="$Comm &gt; 0">
									<xsl:value-of select="$Comm"/>

								</xsl:when>
								<xsl:when test="$Comm &lt; 0">
									<xsl:value-of select="$Comm * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</Commission>

			  <xsl:variable name="SecFee">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL18"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <SecFee>

				  <xsl:choose>
					  <xsl:when test="$Side='S' ">
						  <xsl:choose>


							  <xsl:when test="$SecFee &gt; 0">
								  <xsl:value-of select="$SecFee"/>
							  </xsl:when>

							  <xsl:when test="$SecFee &lt; 0">
								  <xsl:value-of select="$SecFee * (-1)"/>
							  </xsl:when>

							  <xsl:otherwise>
								  <xsl:value-of select="0"/>
							  </xsl:otherwise>

						  </xsl:choose>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>



			  </SecFee>


			  <xsl:variable name="OtherBrokerFee">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="COL14"/>
				  </xsl:call-template>
			  </xsl:variable>

			  <Fees>

				  <xsl:choose>
					  <xsl:when test="$Side='B' ">
						  <xsl:choose>


							  <xsl:when test="$OtherBrokerFee &gt; 0">
								  <xsl:value-of select="$OtherBrokerFee"/>
							  </xsl:when>

							  <xsl:when test="$OtherBrokerFee &lt; 0">
								  <xsl:value-of select="$OtherBrokerFee * (-1)"/>
							  </xsl:when>

							  <xsl:otherwise>
								  <xsl:value-of select="0"/>
							  </xsl:otherwise>

						  </xsl:choose>
					  </xsl:when>

					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>

			  </Fees>
						<!--<xsl:variable name="COL58">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL58"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="COL59">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL59"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="FXRate">
              <xsl:value-of select="$COL58 div $COL59"/>
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
            </FXRate>-->

            <xsl:variable name="NetNotionalValueBase">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL15"/>
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

            <xsl:variable name="NetNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL10"/>
              </xsl:call-template>
            </xsl:variable>

            <NetNotionalValue>

              <xsl:choose>

                <xsl:when test="$NetNotionalValue &gt; 0">
                  <xsl:value-of select="$NetNotionalValue"/>
                </xsl:when>

                <xsl:when test="$NetNotionalValue &lt; 0">
                  <xsl:value-of select="$NetNotionalValue * (-1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>

            </NetNotionalValue>



            <xsl:variable name="GrossNotionalValue">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL16"/>
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
            

            <ISIN>
              <xsl:value-of select="normalize-space(COL18)"/>
            </ISIN>

            <!--<CUSIP>
              <xsl:value-of select="normalize-space(COL9)"/>
            </CUSIP>-->

            <!--<SMRequest>
              <xsl:value-of select="'true'"/>
            </SMRequest>-->

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>