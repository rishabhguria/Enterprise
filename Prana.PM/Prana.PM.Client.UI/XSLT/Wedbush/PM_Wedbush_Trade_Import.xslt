<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="FormatDate">
		<xsl:param name="DateTime"/>
		<!--  converts date time double number to 18/12/2009  -->
		<xsl:variable name="l">
			<xsl:value-of select="$DateTime + 68569 + 2415019"/>
		</xsl:variable>
		<xsl:variable name="n">
			<xsl:value-of select="floor(((4 * $l) div 146097))"/>
		</xsl:variable>
		<xsl:variable name="ll">
			<xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))"/>
		</xsl:variable>
		<xsl:variable name="i">
			<xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))"/>
		</xsl:variable>
		<xsl:variable name="lll">
			<xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31"/>
		</xsl:variable>
		<xsl:variable name="j">
			<xsl:value-of select="floor(((80 * $lll) div 2447))"/>
		</xsl:variable>
		<xsl:variable name="nDay">
			<xsl:value-of select="$lll - floor(((2447 * $j) div 80))"/>
		</xsl:variable>
		<xsl:variable name="llll">
			<xsl:value-of select="floor(($j div 11))"/>
		</xsl:variable>
		<xsl:variable name="nMonth">
			<xsl:value-of select="floor($j + 2 - (12 * $llll))"/>
		</xsl:variable>
		<xsl:variable name="nYear">
			<xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)"/>
		</xsl:variable>
		<xsl:variable name="varMonthUpdated">
			<xsl:choose>
				<xsl:when test="string-length($nMonth) = 1">
					<xsl:value-of select="concat('0',$nMonth)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$nMonth"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="nDayUpdated">
			<xsl:choose>
				<xsl:when test="string-length($nDay) = 1">
					<xsl:value-of select="concat('0',$nDay)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$nDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:value-of select="$varMonthUpdated"/>
		<xsl:value-of select="'/'"/>
		<xsl:value-of select="$nDayUpdated"/>
		<xsl:value-of select="'/'"/>
		<xsl:value-of select="$nYear"/>
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

	<xsl:template name="MonthNo">
		<xsl:param name="varMonth"/>

		<xsl:choose>
			<xsl:when test ="$varMonth= 'A' or $varMonth= 'M'">
				<xsl:value-of select ="1"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'B' or $varMonth= 'N'">
				<xsl:value-of select ="2"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'C' or $varMonth= 'O'">
				<xsl:value-of select ="3"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'D' or $varMonth= 'P'">
				<xsl:value-of select ="4"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'E' or $varMonth= 'Q'">
				<xsl:value-of select ="5"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'F' or $varMonth= 'R'">
				<xsl:value-of select ="6"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'G' or $varMonth= 'S'">
				<xsl:value-of select ="7"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'H' or $varMonth= 'T'">
				<xsl:value-of select ="8"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'I' or $varMonth= 'U'">
				<xsl:value-of select ="9"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'J' or $varMonth= 'V'">
				<xsl:value-of select ="10"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'K' or $varMonth= 'W'">
				<xsl:value-of select ="11"/>
			</xsl:when>
			<xsl:when test ="$varMonth= 'L' or $varMonth= 'X'">
				<xsl:value-of select ="12"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="ConvertBBCodetoTicker">
		<xsl:param name="varBBCode"/>
		<xsl:variable name="varRoot">
			<xsl:choose>
                <xsl:when test="substring(COL5,2,1) = '1'">
                   <xsl:value-of select="substring-before(COL5,'1')"/>
                </xsl:when>
                <xsl:when test="substring(COL5,2,1) = '2'">
                   <xsl:value-of select="substring-before(COL5,'2')"/>
                </xsl:when>
                <xsl:when test="substring(COL5,3,1) = '1'">
                   <xsl:value-of select="substring-before(COL5,'1')"/>
                </xsl:when>
                <xsl:when test="substring(COL5,3,1) = '2'">
                   <xsl:value-of select="substring-before(COL5,'2')"/>
                </xsl:when>
                <xsl:when test="substring(COL5,4,1) = '1'">
                   <xsl:value-of select="substring-before(COL5,'1')"/>
                </xsl:when>
                <xsl:when test="substring(COL5,4,1) = '2'">
                   <xsl:value-of select="substring-before(COL5,'2')"/>
                </xsl:when>
                <xsl:when test="substring(COL5,5,1) = '1'">
                   <xsl:value-of select="substring-before(COL5,'1')"/>
                </xsl:when>
                <xsl:when test="substring(COL5,5,1) = '2'">
                   <xsl:value-of select="substring-before(COL5,'2')"/>
                </xsl:when>
                <xsl:when test="substring(COL5,6,1) = '1'">
                	<xsl:value-of select="substring-before(COL5,'1')"/>
                </xsl:when>
              	<xsl:when test="substring(COL5,6,1) = '2'">
              		<xsl:value-of select="substring-before(COL5,'2')"/>
              	</xsl:when>
            </xsl:choose>
		</xsl:variable>

		<xsl:variable name="varUnderlyingLength">
			<xsl:value-of select="string-length($varRoot)"/>
		</xsl:variable>

		<xsl:variable name="varExYear">
			<xsl:value-of select="substring($varBBCode,($varUnderlyingLength +1),2)"/>
		</xsl:variable>

		<xsl:variable name="varStrike">
			<xsl:value-of select="format-number(substring($varBBCode,($varUnderlyingLength +6)), '#.00')"/>
		</xsl:variable>

		<xsl:variable name="varExDay">
			<xsl:value-of select="substring($varBBCode,($varUnderlyingLength +3),2)"/>
		</xsl:variable>		

		<xsl:variable name="varMonthCode">
			<xsl:value-of select="substring(COL5,($varUnderlyingLength + 5),1)"/>
		</xsl:variable>

		<xsl:variable name="varExMonth">
			<xsl:call-template name="MonthNo">
				<xsl:with-param name="varMonth" select="$varMonthCode"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="varExpiryDay">
			<xsl:choose>
				<xsl:when test="substring($varExDay,1,1)= '0'">
					<xsl:value-of select="substring($varExDay,2,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varExDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:value-of select="normalize-space(concat('O:', $varRoot, ' ', $varExYear,$varMonthCode,$varStrike,'D',$varExpiryDay))"/>

	</xsl:template>
	
	<xsl:template match="/">
		
		<DocumentElement>
			
			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL4)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Position)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Wedbush'"/>
						</xsl:variable>

						<xsl:variable name = "PB_COMPANY_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL2)"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund" />
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="$PRANA_FUND_NAME!=''">
									<xsl:value-of select="$PRANA_FUND_NAME" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_FUND_NAME" />
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>
						
						<SideTagValue>
                          <xsl:choose>
                            <xsl:when test="COL3 = 'P' and COL34 = '2' and not(contains(COL18,'CALL ') or contains(COL18,'PUT '))">
			              	  <xsl:value-of select="'1'"/>
			                </xsl:when>				 
			                <xsl:when test="COL3 = 'P' and COL34 = '6'  and not(contains(COL18,'CALL ') or contains(COL18,'PUT '))">
			              	  <xsl:value-of select="'B'"/>
			                </xsl:when>
			                <xsl:when test="COL3 = 'S' and COL34 = '2'  and not(contains(COL18,'CALL ') or contains(COL18,'PUT '))">
			              	  <xsl:value-of select="'2'"/>
			                </xsl:when>
			                <xsl:when test="COL3 = 'S'and COL34 = '6'   and not(contains(COL18,'CALL ') or contains(COL18,'PUT '))">
			              	  <xsl:value-of select="'5'"/>
			                </xsl:when>
			                <xsl:when test="COL3 = 'SS' and COL34 = '2' and not(contains(COL18,'CALL ') or contains(COL18,'PUT '))">
			              	  <xsl:value-of select="'2'"/>
			                </xsl:when>
			                <xsl:when test="COL3 = 'SS'and COL34 = '6'  and not(contains(COL18,'CALL ') or contains(COL18,'PUT '))">
			              	  <xsl:value-of select="'5'"/>
			                </xsl:when>
			                <!--For option trades-->
			                <xsl:when test="COL3 = 'P' and COL34 = '2' and (contains(COL18,'CALL ') or contains(COL18,'PUT ')) ">
                               <xsl:value-of select="'A'"/>
                             </xsl:when>
			                <xsl:when test="COL3 = 'P' and COL34 = '6' and (contains(COL18,'CALL ') or contains(COL18,'PUT ')) ">
			              	  <xsl:value-of select="'B'"/>
			                </xsl:when>
			                <xsl:when test="COL3 = 'S' and COL34 = '2' and (contains(COL18,'CALL ') or contains(COL18,'PUT ')) ">
			              	  <xsl:value-of select="'D'"/>
			                </xsl:when>
			                <xsl:when test="COL3 = 'S' and COL34 = '6' and (contains(COL18,'CALL ') or contains(COL18,'PUT ')) ">
			              	  <xsl:value-of select="'C'"/>
			                </xsl:when>			           
                             <xsl:when test="COL3 = 'SS'  and (contains(COL18,'CALL ') or contains(COL18,'PUT ')) ">
                               <xsl:value-of select="'C'"/>
                             </xsl:when>                           
                             <xsl:otherwise>
                               <xsl:value-of select="'5'"/>
                             </xsl:otherwise>                
                           </xsl:choose>
                         </SideTagValue>

						<xsl:variable name="varAsset">
							<xsl:value-of select="normalize-space(COL39)"/>
						</xsl:variable>

						 <xsl:variable name ="varCallPut">
				            <xsl:choose>
				        	    <xsl:when test="normalize-space(COL19) = '*' and normalize-space(COL5) != ''">
				        	  	  <xsl:value-of select="1"/>
				        	    </xsl:when>
				        	    <xsl:otherwise>
				        	  	  <xsl:value-of select="0"/>
				        	    </xsl:otherwise>
				            </xsl:choose>
			             </xsl:variable>
						
						<xsl:variable name="varUnderlying">
			        	   <xsl:choose>
			        	    <xsl:when test="normalize-space(COL5) != '' and normalize-space(COL5) != '*' and $varCallPut = 1">
			        	   	  <xsl:value-of select="substring-before(COL5,'1')"/>
			        	    </xsl:when>
			        	    <xsl:otherwise>
			        	   	  <xsl:value-of select="normalize-space(COL5)"/>
			        	    </xsl:otherwise>
			        	   </xsl:choose>
			        	</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varCallPut='1'">
						         <xsl:call-template name="ConvertBBCodetoTicker">
						        	  <xsl:with-param name="varBBCode" select="COL5"/>
						          </xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varUnderlying"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<xsl:variable name="CostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL6)"/>
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
								<xsl:with-param name="Number" select="normalize-space(COL15)"/>
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

		            	<xsl:variable name ="varStampDuty">
		            	   <xsl:choose>
		            	    <xsl:when test ="number(COL12) ">
		            	  	  <xsl:value-of select ="COL12"/>
		            	    </xsl:when>
		            	    <xsl:otherwise>
		            	  	  <xsl:value-of select ="0"/>
		            	    </xsl:otherwise>
		            	   </xsl:choose>
		            	</xsl:variable>
		            		            
		            	<StampDuty>
		            	   <xsl:choose>
		            	     <xsl:when test ="number($varStampDuty) and $varStampDuty &gt; 0">
		            	      <xsl:value-of select ="$varStampDuty"/>
		            	     </xsl:when>
		            	     <xsl:when test ="number($varStampDuty) and $varStampDuty &lt; 0">
		            	      <xsl:value-of select ="$varStampDuty * -1"/>
		            	     </xsl:when>
		            	     <xsl:otherwise>
		            	      <xsl:value-of select ="0"/>
		            	     </xsl:otherwise>
		            	   </xsl:choose>
		            	</StampDuty>
						
						<xsl:variable name="varFees" select="number(COL13)"/>

			            <Fees>
			          	  <xsl:choose>
			          		  <xsl:when test="normalize-space(COL2 = '88522033')">
			          			  <xsl:choose>
			          				  <xsl:when test ="number($varFees) and $varFees &gt; 0">
			          					  <xsl:value-of select ="$varFees"/>
			          				  </xsl:when>
			          				  <xsl:when test ="number($varFees) and $varFees &lt; 0">
			          					  <xsl:value-of select ="$varFees * -1"/>
			          				  </xsl:when>
			          				  <xsl:otherwise>
			          					  <xsl:value-of select ="0"/>
			          				  </xsl:otherwise>
			          			  </xsl:choose>
			          		  </xsl:when>
			          		  <xsl:otherwise>
			          			  <xsl:value-of select="0"/>
			          		  </xsl:otherwise>
			          	  </xsl:choose>
			            </Fees>

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
						
						<CounterPartyID>
			           	    <xsl:value-of select ="122"/>
			            </CounterPartyID>
						
			          	<OriginalPurchaseDate>
			          	    <xsl:value-of select="COL28"/>
			            </OriginalPurchaseDate>
			  
						<PositionStartDate>
							<xsl:value-of select="normalize-space(COL26)"/>
						</PositionStartDate>

						<PositionSettlementDate>
							<xsl:value-of select="normalize-space(COL27)"/>
						</PositionSettlementDate>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


