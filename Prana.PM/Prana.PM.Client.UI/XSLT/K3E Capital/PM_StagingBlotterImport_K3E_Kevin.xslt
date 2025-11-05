<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation"></xsl:attribute>
      <xsl:for-each select="//PositionMaster">

        <xsl:variable name ="varSide">
          <xsl:value-of select ="translate(COL1,'&quot;','')"/>
        </xsl:variable>

        <xsl:if test="$varSide != 'Side' and translate(normalize-space(COL2),'&quot;','')!='USD' and translate(normalize-space(COL2),'&quot;','')!='CASH'">
          <PositionMaster>
            <OrderSideTagValue>
              <xsl:choose>
                <xsl:when test="$varSide='Purchase'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$varSide='Buy'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:when test="$varSide='Sale'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:when test="$varSide='Sell'">
                  <xsl:value-of select="'2'"/>
                </xsl:when>
                <xsl:when test="$varSide='Sell Short'">
                  <xsl:value-of select="'5'"/>
                </xsl:when>
                <xsl:when test="$varSide='Buy to Close'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>
                <xsl:when test="$varSide='Buy To Close'">
                  <xsl:value-of select="'B'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </OrderSideTagValue>


            <Symbol>
              <xsl:value-of select="normalize-space(COL2)"/>
            </Symbol>

            <xsl:variable name ="varQuantity">
              <xsl:value-of select ="COL3"/>
            </xsl:variable>

            <Quantity>
              <xsl:choose>
                <xsl:when test="number($varQuantity)">
                  <xsl:value-of select="number($varQuantity)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Quantity>

            <!--<AuecLocalDate>
						<xsl:choose>
								<xsl:when test="COL8='*' or COL8=''">
									<xsl:value-of select="'2014-03-06 08:43:29.000'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL8"/>
								</xsl:otherwise>
							</xsl:choose>
						</AuecLocalDate>-->

            <xsl:variable name ="varOrderTypeTagValue">
              <xsl:value-of select ="translate(normalize-space(COL4),'&quot;','')"/>
            </xsl:variable>
            <OrderTypeTagValue>

              <xsl:choose>

                <xsl:when test ="$varOrderTypeTagValue='Market'">
                  <xsl:value-of select ="'1'"/>
                </xsl:when>
                <xsl:when test ="contains($varOrderTypeTagValue,'Limit')">
                  <xsl:value-of select ="'2'"/>
                </xsl:when>
                <xsl:when test ="$varOrderTypeTagValue='Stop'">
                  <xsl:value-of select ="'3'"/>
                </xsl:when>
                <xsl:when test ="$varOrderTypeTagValue='Stop Limit'">
                  <xsl:value-of select ="'4'"/>
                </xsl:when>
                <xsl:when test ="$varOrderTypeTagValue='Market on close'">
                  <xsl:value-of select ="'5'"/>
                </xsl:when>
                <xsl:when test ="$varOrderTypeTagValue='With or without'">
                  <xsl:value-of select ="'6'"/>
                </xsl:when>
                <xsl:when test ="$varOrderTypeTagValue='Limit or better'">
                  <xsl:value-of select ="'7'"/>
                </xsl:when>
                <xsl:when test ="$varOrderTypeTagValue='Limit with or without'">
                  <xsl:value-of select ="'8'"/>
                </xsl:when>
                <xsl:when test ="$varOrderTypeTagValue='On basis'">
                  <xsl:value-of select ="'9'"/>
                </xsl:when>
                <xsl:when test ="$varOrderTypeTagValue='Pegged'">
                  <xsl:value-of select ="'P'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="'1'"/>
                </xsl:otherwise>
              </xsl:choose>
            </OrderTypeTagValue>

            <xsl:variable name ="varPrice">
              <xsl:value-of select ="translate(COL5,'&quot;','')"/>
            </xsl:variable>
            <Price>
              <xsl:choose>
                <xsl:when test ="contains($varOrderTypeTagValue,'Limit')">
                  <xsl:value-of select="$varPrice"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>				
           </Price>

            <!--<UserName>
              <xsl:value-of select="COL8"/>
            </UserName>-->

			<Level1ID>
				<xsl:value-of select ="10136"/>
			</Level1ID>

            <ExecutionInstruction>
							<xsl:value-of select="'E'"/>
						</ExecutionInstruction>


            <xsl:variable name ="varVenue">
              <xsl:value-of select ="translate(normalize-space(COL9),'&quot;','')"/>
            </xsl:variable>
			  <Venue>
				  <xsl:value-of select ="$varVenue"/>
			  </Venue>

			  <VenueID>
          <xsl:choose>
            <xsl:when test="$varVenue='Drops'">
              <xsl:value-of select="'1'"/>
            </xsl:when>
            <xsl:when test="$varVenue='Algo'">
              <xsl:value-of select="'41'"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="0"/>
            </xsl:otherwise>
          </xsl:choose>				  
			  </VenueID>

			  <UserID>
             <xsl:value-of select ="'61'"/>
           </UserID>

            <xsl:variable name="varCounterPartyName">
              <xsl:value-of select ="translate(normalize-space(COL10),'&quot;','')"/>
            </xsl:variable>
            <CounterPartyName>
              <xsl:value-of select ="$varCounterPartyName"/>
            </CounterPartyName>

            <CounterPartyID>
              <xsl:choose>
                <xsl:when test="$varCounterPartyName='CANT'">
                  <xsl:value-of select="15"/>
                </xsl:when>
                <xsl:when test="$varCounterPartyName='COWN'">
                  <xsl:value-of select="19"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>

            </CounterPartyID>

            
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>