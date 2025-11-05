<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:my="put-your-namespace-uri-here">

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

	<xsl:template match="/">

		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="varQuantity">
					<xsl:value-of select="normalize-space(COL6)"/>
				</xsl:variable>		

				<xsl:if test="number($varQuantity)">

					<PositionMaster>
						
						<TickerSymbol>
							<xsl:value-of select="normalize-space(COL2)"/>
						</TickerSymbol>

						<xsl:variable name="varMonthCodevar">
							<xsl:value-of select="substring(substring-after(normalize-space(COL2),' '),3,1)"/>
						</xsl:variable>
						
						<xsl:variable name="varPutORCall">
			               <xsl:choose>
			               	<xsl:when test="$varMonthCodevar ='A'">
			               		<xsl:value-of select="'C'"/>
			               	</xsl:when>
			               	<xsl:when test="$varMonthCodevar ='B'">
			               		<xsl:value-of select="'C'"/>
			               	</xsl:when>
			               	<xsl:when test="$varMonthCodevar ='C'">
			               		<xsl:value-of select="'C'"/>
			               	</xsl:when>
			               	<xsl:when test="$varMonthCodevar ='D'">
			               		<xsl:value-of select="'C'"/>
			               	</xsl:when>
			               	<xsl:when test="$varMonthCodevar ='E'">
			               		<xsl:value-of select="'C'"/>
			               	</xsl:when>
			               	<xsl:when test="$varMonthCodevar ='F'">
			               		<xsl:value-of select="'C'"/>
			               	</xsl:when>
			               	<xsl:when test="$varMonthCodevar ='G'">
			               		<xsl:value-of select="'C'"/>
			               	</xsl:when>
			               	<xsl:when test="$varMonthCodevar ='H'">
			               		<xsl:value-of select="'C'"/>
			               	</xsl:when>
			               	<xsl:when test="$varMonthCodevar ='I'">
			               		<xsl:value-of select="'C'"/>
			               	</xsl:when>
			               	<xsl:when test="$varMonthCodevar ='J'">
			        		<xsl:value-of select="'C'"/>
			        	   </xsl:when>
			        	   <xsl:when test="$varMonthCodevar ='K' ">
			        	   	<xsl:value-of select="'C'"/>
			        	   </xsl:when>
			        	   <xsl:when test="$varMonthCodevar ='L' ">
			        	   	<xsl:value-of select="'C'"/>
			        	   </xsl:when>
			        	   <xsl:when test="$varMonthCodevar ='M'">
			        	   	<xsl:value-of select="'P'"/>
			        	   </xsl:when>
			        	   <xsl:when test="$varMonthCodevar ='N'">
			        	   	<xsl:value-of select="'P'"/>
			        	   </xsl:when>
			        	   <xsl:when test="$varMonthCodevar ='O'">
			        	   	<xsl:value-of select="'P'"/>
			        	   </xsl:when>
			        	   <xsl:when test="$varMonthCodevar ='P'">
			        	   	<xsl:value-of select="'P'"/>
			        	   </xsl:when>
			        	   <xsl:when test="$varMonthCodevar ='Q'">
			        	   	<xsl:value-of select="'P'"/>
			        	   </xsl:when>
			        	   <xsl:when test="$varMonthCodevar ='R'">
			        	   	<xsl:value-of select="'P'"/>
			        	   </xsl:when>
			        	   <xsl:when test="$varMonthCodevar ='S'">
			        	   	<xsl:value-of select="'P'"/>
			        	   </xsl:when>
			        	   <xsl:when test="$varMonthCodevar ='T'">
			        	   	<xsl:value-of select="'P'"/>
			        	   </xsl:when>
			        	   <xsl:when test="$varMonthCodevar ='U'">
			             	   	<xsl:value-of select="'P'"/>
			             	   </xsl:when>
		              	   <xsl:when test="$varMonthCodevar ='V'">
		              			<xsl:value-of select="'P'"/>
		              		</xsl:when>
		              		<xsl:when test="$varMonthCodevar ='W'">
		              			<xsl:value-of select="'P'"/>
		              		</xsl:when>
		              		<xsl:when test="$varMonthCodevar ='X'">
		              			<xsl:value-of select="'P'"/>
		              		</xsl:when>
		              		<xsl:otherwise>
		              			<xsl:value-of select="''"/>
		              		</xsl:otherwise>
		              	</xsl:choose>
		              </xsl:variable>

						<PutOrCall>
							<xsl:choose>
								<xsl:when test="$varPutORCall='P'">
									<xsl:value-of select="'0'"/>
								</xsl:when>
								<xsl:when test="$varPutORCall='C'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'-1'"/>
								</xsl:otherwise>
							</xsl:choose>
						</PutOrCall>

						<Multiplier>
							<xsl:value-of select="100"/>
						</Multiplier>							

						<xsl:variable name="varExpiryDay">
							<xsl:choose>
								<xsl:when test="string-length(substring-after(substring-after(substring-after(normalize-space(COL2),' '),'.'),'D')) = 2">
									<xsl:value-of select="substring(substring-after(substring-after(substring-after(normalize-space(COL2),' '),'.'),'D'),1,2)"/>
								</xsl:when>	
								<xsl:when test="string-length(substring-after(substring-after(substring-after(normalize-space(COL2),' '),'.'),'D')) = 1">
									<xsl:value-of select="concat('0',substring-after(substring-after(substring-after(normalize-space(COL2),' '),'.'),'D'))"/>
								</xsl:when>	
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						<xsl:variable name="varExpiryMonth">
			               <xsl:choose>
			               	<xsl:when test="$varMonthCodevar ='A' or $varMonthCodevar ='M'">
			               		<xsl:value-of select="'01'"/>
			               	</xsl:when>
			               	<xsl:when test="$varMonthCodevar ='B' or $varMonthCodevar ='N'">
			               		<xsl:value-of select="'02'"/>
			               	</xsl:when>
			               	<xsl:when test="$varMonthCodevar ='C' or $varMonthCodevar ='O'">
			               		<xsl:value-of select="'03'"/>
			               	</xsl:when>
			               	<xsl:when test="$varMonthCodevar ='D' or $varMonthCodevar ='P'">
			               		<xsl:value-of select="'04'"/>
			               	</xsl:when>
			               	<xsl:when test="$varMonthCodevar ='E' or $varMonthCodevar ='Q'">
			               		<xsl:value-of select="'05'"/>
			               	</xsl:when>
			               	<xsl:when test="$varMonthCodevar ='F' or $varMonthCodevar ='R'">
			               		<xsl:value-of select="'06'"/>
			               	</xsl:when>
			               	<xsl:when test="$varMonthCodevar ='G' or $varMonthCodevar ='S'">
			               		<xsl:value-of select="'07'"/>
			               	</xsl:when>
			               	<xsl:when test="$varMonthCodevar ='H' or $varMonthCodevar ='T'">
			               		<xsl:value-of select="'08'"/>
			               	</xsl:when>
			               	<xsl:when test="$varMonthCodevar ='I' or $varMonthCodevar ='U'">
			               		<xsl:value-of select="'09'"/>
			               	</xsl:when>
			               	<xsl:when test="$varMonthCodevar ='J' or $varMonthCodevar ='V'">
			               		<xsl:value-of select="'10'"/>
			               	</xsl:when>
		            		<xsl:when test="$varMonthCodevar ='K' or $varMonthCodevar ='W'">
		            			<xsl:value-of select="'11'"/>
		            		</xsl:when>
		            		<xsl:when test="$varMonthCodevar ='L' or $varMonthCodevar ='X'">
		            			<xsl:value-of select="'12'"/>
		            		</xsl:when>
		            		<xsl:otherwise>
		            			<xsl:value-of select="''"/>
		            		</xsl:otherwise>
		                	</xsl:choose>
		                </xsl:variable>
						
						<xsl:variable name="varExpiryYear">
							<xsl:value-of select="concat('20',substring(substring-after(normalize-space(COL2),' '),1,2))"/>
						</xsl:variable>

						<xsl:variable name="varExpiryDate">
							<xsl:value-of select="concat($varExpiryMonth,'/',$varExpiryDay,'/',$varExpiryYear)"/>
						</xsl:variable>						

						<ExpirationDate>
							<xsl:value-of select="$varExpiryDate"/>
						</ExpirationDate>
												
						<xsl:variable name="UnderlyingSymbol">
							<xsl:value-of select="substring-before(substring-after(normalize-space(COL2),':'),' ')"/>
						</xsl:variable>

						<UnderLyingSymbol>
							<xsl:value-of select="$UnderlyingSymbol"/>
						</UnderLyingSymbol>
						
						<xsl:variable name="StrikePriceBeforeDecimal">
							<xsl:value-of select="substring-before(substring(substring-after(normalize-space(COL2),' '),4),'.')"/>
						</xsl:variable>

						<xsl:variable name="StrikePriceAfterDecimal">
							<xsl:value-of select="substring(substring-after(normalize-space(COL2),'.'),1,2)"/>
						</xsl:variable>

						<xsl:variable name="StrikePrice">
							<xsl:value-of select="concat($StrikePriceBeforeDecimal,'.',$StrikePriceAfterDecimal)"/>
						</xsl:variable>						                     
						
						<StrikePrice>
							<xsl:choose>
								<xsl:when test="number($StrikePrice)">
									<xsl:value-of select="$StrikePrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</StrikePrice>

						<xsl:variable name="StrikePrice2">
							<xsl:value-of select="$StrikePrice * 1000"/>
						</xsl:variable>

						<xsl:variable name="StrikePrice1">
							<xsl:choose>
								<xsl:when test="string-length($StrikePrice2)=7">
									<xsl:value-of select="concat('0',$StrikePrice2)"/>
								</xsl:when>
								<xsl:when test="string-length($StrikePrice2)=6">
									<xsl:value-of select="concat('00',$StrikePrice2)"/>
								</xsl:when>
								<xsl:when test="string-length($StrikePrice2)=5">
									<xsl:value-of select="concat('000',$StrikePrice2)"/>
								</xsl:when>
								<xsl:when test="string-length($StrikePrice2)=4">
									<xsl:value-of select="concat('0000',$StrikePrice2)"/>
								</xsl:when>
								<xsl:when test="string-length($StrikePrice2)=3">
									<xsl:value-of select="concat('00000',$StrikePrice2)"/>
								</xsl:when>
								<xsl:when test="string-length($StrikePrice2)=2">
									<xsl:value-of select="concat('000000',$StrikePrice2)"/>
								</xsl:when>
								<xsl:when test="string-length($StrikePrice2)=1">
									<xsl:value-of select="concat('0000000',$StrikePrice2)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$StrikePrice2"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varExpiryYearOSI">
							<xsl:value-of select="substring(substring-after(normalize-space(COL2),' '),1,2)"/>
						</xsl:variable>

						<xsl:variable name="OsiOption">
							<xsl:choose>
								<xsl:when test="string-length($UnderlyingSymbol)=1">
									<xsl:value-of select="concat($UnderlyingSymbol,' ',' ',' ',' ',' ',$varExpiryYearOSI,$varExpiryMonth,$varExpiryDay,$varPutORCall,$StrikePrice1)"/>
								</xsl:when>
								<xsl:when test="string-length($UnderlyingSymbol)=2">
									<xsl:value-of select="concat($UnderlyingSymbol,' ',' ',' ',' ',$varExpiryYearOSI,$varExpiryMonth,$varExpiryDay,$varPutORCall,$StrikePrice1)"/>
								</xsl:when>
								<xsl:when test="string-length($UnderlyingSymbol)=3">
									<xsl:value-of select="concat($UnderlyingSymbol,' ',' ',' ',$varExpiryYearOSI,$varExpiryMonth,$varExpiryDay,$varPutORCall,$StrikePrice1)"/>
								</xsl:when>
								<xsl:when test="string-length($UnderlyingSymbol)=4">
									<xsl:value-of select="concat($UnderlyingSymbol,' ',' ',$varExpiryYearOSI,$varExpiryMonth,$varExpiryDay,$varPutORCall,$StrikePrice1)"/>
								</xsl:when>
								<xsl:when test="string-length($UnderlyingSymbol)=5">
									<xsl:value-of select="concat($UnderlyingSymbol,' ',$varExpiryYearOSI,$varExpiryMonth,$varExpiryDay,$varPutORCall,$StrikePrice1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						<LongName>
							<xsl:value-of select="$OsiOption"/>
						</LongName>

						<OSIOptionSymbol>
							<xsl:value-of select="$OsiOption"/>
						</OSIOptionSymbol>

						<IDCOOptionSymbol>
							<xsl:value-of select="concat($OsiOption,'U')"/>
						</IDCOOptionSymbol>

						<AUECID>
							<xsl:value-of select="12"/>
						</AUECID>

						<UDASector>
							<xsl:value-of select="'Undefined'"/>
						</UDASector>

						<UDASubSector>
							<xsl:value-of select="'Undefined'"/>
						</UDASubSector>

						<UDASecurityType>
							<xsl:value-of select="'Undefined'"/>
						</UDASecurityType>

						<UDAAssetClass>
							<xsl:value-of select="'Undefined'"/>
						</UDAAssetClass>

						<UDACountry>
							<xsl:value-of select="'Undefined'"/>
						</UDACountry>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>