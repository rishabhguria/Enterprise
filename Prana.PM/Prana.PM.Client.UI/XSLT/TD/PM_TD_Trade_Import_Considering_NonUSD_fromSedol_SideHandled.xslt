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
		<xsl:if test="contains(COL5,'Option') or contains(COL2,'CALL') ">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(COL4,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(COL4,'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL4),' '),' '),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-before(substring-after(substring-after(normalize-space(COL4),'/'),'/'),' '),3,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL4),'/'),'/'),' '),' '),1,1)"/>
			</xsl:variable>
			
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL4),'/'),'/'),' '),' '),2),'#.00')"/>
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

				
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
			
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

        <xsl:variable name="NetPosition">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL10"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:if test="number($NetPosition)">
			
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'TD Securities'"/>
            </xsl:variable>

            <xsl:variable name="PB_SYMBOL_NAME">
              <xsl:value-of select="normalize-space(COL6)"/>
            </xsl:variable>
            
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
            </xsl:variable>
			
			<xsl:variable name="varSymbol">
              <xsl:value-of select="normalize-space(COL6)"/>
            </xsl:variable>

			  <xsl:variable name="varAsset">
				  <xsl:choose>
					  <xsl:when test="contains(COL5,'Option')">
						  <xsl:value-of select="'EquityOption'"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="'Equity'"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>
			  
			  <xsl:variable name="varSedol1">
                            <xsl:choose>
                                <xsl:when test="string-length(normalize-space(COL20)) = '7'">
                                    <xsl:value-of select="normalize-space(COL20)"/>
                                </xsl:when>
                                <xsl:when test="string-length(normalize-space(COL20)) = '6'">
                                    <xsl:value-of select="concat(0,COL20)"/>
                                </xsl:when>
                                <xsl:when test="string-length(normalize-space(COL20)) = '5'">
                                    <xsl:value-of select="concat('00',normalize-space(COL20))"/>
                                </xsl:when>
                                <xsl:otherwise>
                                    <xsl:value-of select="''"/>
                                </xsl:otherwise>
                            </xsl:choose>
                        </xsl:variable>

                        <xsl:variable name="varSedol">
                            <xsl:choose>
                                <xsl:when test="normalize-space(COL2) != 'USD'">
                                    <xsl:value-of select="$varSedol1"/>
                                </xsl:when>
                                <xsl:otherwise>
                                    <xsl:value-of select="''"/>
                                </xsl:otherwise>
                            </xsl:choose>
                        </xsl:variable>

					<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varAsset='EquityOption'">
									<xsl:call-template name="Option">
										<xsl:with-param name="Symbol" select="COL4"/>
										<xsl:with-param name="Suffix" select="''"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:when test="$varSedol != ''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$varSymbol != ''">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

            
			<SEDOL>
                 <xsl:choose>
                     <xsl:when test="$varSedol != ''">
                         <xsl:value-of select="$varSedol"/>
                     </xsl:when>
                     <xsl:otherwise>
                         <xsl:value-of select="''"/>
                     </xsl:otherwise>
                 </xsl:choose>
             </SEDOL>
            <xsl:variable name="PB_FUND_NAME" select="COL1"/>

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
			  
			<TradeAttribute3>
			  <xsl:choose>
                <xsl:when test ="$PRANA_FUND_NAME!=''">
                  <xsl:value-of select ="$PRANA_FUND_NAME"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$PB_FUND_NAME"/>
                </xsl:otherwise>
              </xsl:choose>
			</TradeAttribute3>

            <xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="normalize-space(COL19)"/>
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

            <NetPosition>
              <xsl:choose>
                <xsl:when test="$NetPosition &gt; 0">
                  <xsl:value-of select="$NetPosition"/>
                </xsl:when>
                <xsl:when test="$NetPosition &lt; 0">
                  <xsl:value-of select="$NetPosition* (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetPosition>

            <xsl:variable name="CostBasis">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL11"/>
              </xsl:call-template>
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


            <xsl:variable name="Commission">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL14"/>
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

            <xsl:variable name="Secfee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL12"/>
              </xsl:call-template>
            </xsl:variable>
			  
            <SecFee>
              <xsl:choose>
                <xsl:when test="$Secfee &gt; 0">
                  <xsl:value-of select="$Secfee"/>

                </xsl:when>
                <xsl:when test="$Secfee &lt; 0">
                  <xsl:value-of select="$Secfee * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </SecFee>

            <xsl:variable name ="Side" select="COL9"/>
			  
            <SideTagValue>

				<xsl:choose>
					<xsl:when test="$varAsset='EquityOption'">
						<xsl:choose>
							<xsl:when test="$Side='BUY TO COVER' or $Side='BUY TO CLOSE'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="$Side='BUY'">
								<xsl:value-of select="'A'"/>
							</xsl:when>
							<xsl:when test="$Side='SELL'">
								<xsl:value-of select="'D'"/>
							</xsl:when>
							<xsl:when test="$Side='SHORT SELL'">
								<xsl:value-of select="'C'"/>
							</xsl:when>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="$Side='BUY' and COL25 ='S' ">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="$Side='SELL' and COL25 ='S'">
								<xsl:value-of select="'5'"/>
							</xsl:when>
							<xsl:when test="$Side='BUY' and COL25 !='S'">
								<xsl:value-of select="'1'"/>
							</xsl:when>
							<xsl:when test="$Side='SELL' and COL25 !='S'">
								<xsl:value-of select="'2'"/>
							</xsl:when>
							
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>           
            </SideTagValue>

            <xsl:variable name="FXRate">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL16"/>
              </xsl:call-template>
            </xsl:variable>

			  <xsl:variable name="varFxrate">
				  <xsl:choose>
					  <xsl:when test="normalize-space(COL15)='GBP' or normalize-space(COL15)='EUR' or normalize-space(COL15)='AUD' or normalize-space(COL15)='NZD'">
						  <xsl:value-of select="$FXRate"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="1 div $FXRate"/>
					  </xsl:otherwise>
				  </xsl:choose>
			 </xsl:variable>
            <FXRate>
              <xsl:choose>
                <xsl:when test="number($varFxrate)">
                  <xsl:value-of select="$varFxrate"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </FXRate>

            <PBSymbol>
              <xsl:value-of select="$PB_SYMBOL_NAME"/>
            </PBSymbol>

			  <xsl:variable name="varSDate">
				  <xsl:value-of select="COL8"/>
			  </xsl:variable>
            <PositionSettlementDate>
              <xsl:value-of select="$varSDate"/>
            </PositionSettlementDate>

			  <xsl:variable name="varDate">
				  <xsl:value-of select="COL7"/>
			  </xsl:variable>
            <PositionStartDate>
              <xsl:value-of select="$varDate"/>
            </PositionStartDate>			 

			<xsl:variable name="ClearingFees">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="normalize-space(COL13)"/>
              </xsl:call-template>
            </xsl:variable>

			  <ClearingFee>
				  <xsl:choose>
					  <xsl:when test="$ClearingFees &gt; 0">
						  <xsl:value-of select="$ClearingFees"/>
					  </xsl:when>
					  <xsl:when test="$ClearingFees &lt; 0">
						  <xsl:value-of select="$ClearingFees * (-1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="0"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </ClearingFee>
			  
			  <xsl:variable name="OtherBrokerCalc">
				  <xsl:value-of select="$NetPosition * $CostBasis * 100"/>
			  </xsl:variable>

			  <xsl:variable name="OtherBrokerCalc1">
				  <xsl:value-of select="(COL12 + COL13)+ COL14"/>
			  </xsl:variable>

			 

			  <xsl:variable name="FinalOBF">
				  <xsl:value-of select="($OtherBrokerCalc - $OtherBrokerCalc1 + COL17)"/>
			  </xsl:variable>
			  
			  <xsl:variable name="OtherBrokerFee">
				  <xsl:call-template name="Translate">
					  <xsl:with-param name="Number" select="$FinalOBF"/>
				  </xsl:call-template>
			  </xsl:variable>
			  
			  <Fees>
				  <xsl:choose>
					  <xsl:when test="$varAsset='EquityOption'">
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

          </PositionMaster>

        </xsl:if>
      </xsl:for-each>
    </DocumentElement>

  </xsl:template>

  <xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>